using docuverwalter_api.Models;

namespace docuverwalter_api.Services.DocumentService
{
    public interface IDocumentService
    {
        Task<Document> GetDocumentByIdAsync(Guid id);
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<Document> UploadDocumentAsync(Document document);
        Task<Document> UpdateDocumentAsync(Guid id, Document document);
        Task<bool> DeleteDocumentAsync(Guid id);
        //Task<Document> GetDocumentByBlobNameAsync(string blobName);
        Task<IEnumerable<Document>> GetDocumentsByIdsAsync(List<Guid> documentIds);

    }
}
