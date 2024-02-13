using docuverwalter_api.Data;
using docuverwalter_api.Models;
using Microsoft.EntityFrameworkCore;



namespace docuverwalter_api.Services.DocumentService
{
    public class DocumentShareLinkService : IDocumentShareLinkService
    {

        private readonly ApplicationDbContext _context;

        private readonly string _baseUrl;

        public DocumentShareLinkService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;

            _baseUrl = configuration["ApplicationSettings:BaseUrl"]; 
        }
        public async Task<DocumentShareLink> CreateShareLinkAsync(Guid documentId, DateTime expiryDateTime)
        {
          
            var shareLink = new DocumentShareLink
            {
                ShareLinkId = Guid.NewGuid(),
                DocumentId = documentId,
                GeneratedLink = GenerateUniqueLink(documentId), 
                ExpiryDateTime = expiryDateTime,
                IsActive = true
            };

            _context.DocumentShareLinks.Add(shareLink);
            await _context.SaveChangesAsync();

            return shareLink;
        }

      

        public async Task<DocumentShareLink> GetShareLinkByLinkAsync(string uniqueToken)
        {
            return await _context.DocumentShareLinks
                                 .FirstOrDefaultAsync(sl => sl.GeneratedLink.EndsWith(uniqueToken) &&
                                                            sl.IsActive &&
                                                            sl.ExpiryDateTime >= DateTime.Now);
        }



        private string GenerateUniqueLink(Guid documentId)
        {
            var uniqueToken = Guid.NewGuid().ToString();
            return $"{_baseUrl}/api/documentShareLink/access/{uniqueToken}";
        }

    }
}
