using docuverwalter_api.Models;

namespace docuverwalter_api.Services.DocumentService
{
    public interface IDocumentShareLinkService
    {
        Task<DocumentShareLink> CreateShareLinkAsync(Guid documentId, DateTime expiryDateTime);
        Task<DocumentShareLink> GetShareLinkByLinkAsync(string shareLink);
    }
}
