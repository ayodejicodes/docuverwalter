using AutoMapper;
using docuverwalter_api.Models;
using docuverwalter_api.Models.Dtos.DocumentDto;
using docuverwalter_api.Services.BlobStorageService;
using docuverwalter_api.Services.DocumentService;
using docuverwalter_api.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Net;

namespace doku_speicher_api.Controllers
{

    [Route("api/document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {

        private readonly IDocumentService _documentService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocumentController(IDocumentService documentService, IBlobStorageService blobStorageService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _documentService = documentService;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Document>>>> GetAllDocuments()
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(ApiResponse<IEnumerable<Document>>.Success(documents));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ApiResponse<Document>>> GetDocument(Guid id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound(ApiResponse<Document>.Failure(new List<string> { "Document not found" }));
            }

            return Ok(ApiResponse<Document>.Success(document));
        }


        [HttpPost("upload")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<Document>>>> CreateDocuments([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(ApiResponse<IEnumerable<Document>>.Failure(new List<string> { "No files attached" }));
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not Authorized!");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var documents = new List<Document>();

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        continue;
                    }

                    var fileName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName).TrimStart('.');
                    var fileSize = file.Length;
                    var filePath = await _blobStorageService.UploadBlobAsync(file);

                  
                    var document = _mapper.Map<Document>(new DocumentCreateDto());
                    document.UploadDateTime = DateTime.Now;
                    document.DocumentName = fileName;
                    document.FilePath = filePath;
                    document.DocumentType = fileExtension;
                    document.FileSize = fileSize;


                    document.ApplicationUser = user;

                    var createdDocument = await _documentService.UploadDocumentAsync(document);
                    documents.Add(createdDocument);
                }

                return CreatedAtAction(nameof(GetAllDocuments), ApiResponse<IEnumerable<Document>>.Success(documents));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Document>>.Failure(new List<string> { $"An error occurred while creating documents:{ex}" }, HttpStatusCode.InternalServerError));
            }
        }



        [HttpPut("edit/{id:Guid}")]
        public async Task<ActionResult<ApiResponse<Document>>> UpdateDocument(Guid id, [FromBody] DocumentUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<Document>.Failure(new List<string> { "Invalid data" }));
            }

            try
            {
                var existingDocument = await _documentService.GetDocumentByIdAsync(id);
                if (existingDocument == null)
                {
                    return NotFound(ApiResponse<Document>.Failure(new List<string> { "Document not found" }));
                }

                var fileExtension = Path.GetExtension(existingDocument.DocumentType);

                _mapper.Map(updateDto, existingDocument);
                existingDocument.DocumentName = $"{updateDto.DocumentName}{fileExtension}";

                existingDocument.LastEditedTime = DateTime.Now;

                existingDocument = await _documentService.UpdateDocumentAsync(id, existingDocument);

                return Ok(ApiResponse<Document>.Success(existingDocument));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Document>.Failure(new List<string> { $"An error occurred while updating the document:{ex}" }));
            }
        }



        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteDocument(Guid id)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(id);
                if (document == null)
                {
                    return NotFound(ApiResponse<bool>.Failure(new List<string> { "Document not found" }));
                }

                if (!string.IsNullOrEmpty(document.FilePath))
                {

                    var blobName = FileUtility.ExtractBlobNameFromPath(document.FilePath);
                    var blobDeleted = await _blobStorageService.DeleteBlobAsync(blobName);
                    if (!blobDeleted)
                    {
                        return StatusCode(500, ApiResponse<Document>.Failure(new List<string> { "Failed to delete document." }));
                    }
                }

                await _documentService.DeleteDocumentAsync(id);
                return Ok(ApiResponse<bool>.Success(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Failure(new List<string> { $"An error occurred while deleting the document:{ex}" }));
            }
        }



        [HttpDelete("delete/multiple")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMultipleDocuments([FromQuery] List<Guid>? documentIds = null)
        {
            if (documentIds == null || !documentIds.Any())
            {
                return BadRequest("No document IDs provided.");
            }

            var deletionErrors = new List<string>();

            foreach (var documentId in documentIds)
            {
                try
                {
                    var deletionResult = await DeleteDocument(documentId);
                    if (!(deletionResult.Result is OkObjectResult))
                    {
                        deletionErrors.Add($"Failed to delete document with ID {documentId}.");
                    }
                }
                catch (Exception ex)
                {
                    deletionErrors.Add($"Error deleting document with ID {documentId}: {ex.Message}");
                }
            }

            if (deletionErrors.Any())
            {
                return StatusCode(500, ApiResponse<bool>.Failure(deletionErrors, HttpStatusCode.InternalServerError));
            }

            return Ok( "All documents deleted successfully.");
        }



        [HttpGet("download")]
        public async Task<IActionResult> DownloadDocuments([FromQuery] List<Guid>? documentIds = null)
        {
            try
            {
                if (documentIds == null || documentIds.Count == 0)
                {
                    return BadRequest("No documents selected for download.");
                }

            
                var documents = await _documentService.GetDocumentsByIdsAsync(documentIds);
                if (documents == null)
                {
                    return NotFound("Documents not found.");
                }

                if (documents.Count() == 1)
                {
                    var document = documents.First();
                    var blobName = Path.GetFileName(document.FilePath);
                    var stream = await _blobStorageService.GetBlobAsync(blobName);
                    if (stream == null)
                    {
                        return NotFound("File not found in the blob storage.");
                    }

                    document.NumberOfDownloads += 1;
                    await _documentService.UpdateDocumentAsync(document.DocumentId, document);

                    string contentType = "application/octet-stream";
                    return File(stream, contentType, document.DocumentName);
                }
                else
                {
                    var memoryStream = new MemoryStream();
                    using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var document in documents)
                        {
                            var blobName = Path.GetFileName(document.FilePath);
                            var stream = await _blobStorageService.GetBlobAsync(blobName);
                            if (stream != null)
                            {
                                var entry = zipArchive.CreateEntry(document.DocumentName);
                                using (var entryStream = entry.Open())
                                {
                                    await stream.CopyToAsync(entryStream);
                                }

                                document.NumberOfDownloads += 1;
                                await _documentService.UpdateDocumentAsync(document.DocumentId, document);
                            }
                        }
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var contentType = "application/zip";
                    var archiveName = "downloaded_documents.zip";

                    return File(memoryStream, contentType, archiveName);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while attempting to download documents: {ex.Message}");
            }
        }

    }
}
