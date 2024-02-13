namespace docuverwalter_api.Services.BlobStorageService
{
    public interface IBlobStorageService
    {
        Task<string> UploadBlobAsync(IFormFile file);
        Task<bool> DeleteBlobAsync(string blobName);
        Task<Stream> GetBlobAsync(string blobName);
    }
}
