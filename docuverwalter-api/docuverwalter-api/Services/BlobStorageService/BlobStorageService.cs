using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using docuverwalter_api.Services.BlobStorageService;

namespace docuverwalter_api.Services.BlobStorageService
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string? _containerName;


        public BlobStorageService(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]);
            _containerName = configuration["AzureBlobStorage:ContainerName"];
        }


        public async Task<string> UploadBlobAsync(IFormFile file)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobName = Guid.NewGuid().ToString() + "-" + file.FileName;
                var blobClient = containerClient.GetBlobClient(blobName);

                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
                }

                return blobClient.Uri.ToString();

            }
            catch (Exception ex)
            {
                return $"Upload Failed: {ex}";
            }
        }

        public async Task<bool> DeleteBlobAsync(string blobName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(blobName);
                var response = await blobClient.DeleteIfExistsAsync();
                return response.Value;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Stream> GetBlobAsync(string blobName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(blobName);
                var blobDownloadInfo = await blobClient.DownloadAsync();
                return blobDownloadInfo.Value.Content;
            }
            catch (Exception ex)
            {

                return Stream.Null;
            }
        }

    }
}
