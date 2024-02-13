using docuverwalter_api.Data;
using docuverwalter_api.Models;
using docuverwalter_api.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace docuverwalter_api.Services.DocumentService
{
    public class DocumentService: IDocumentService
    {

        private readonly ApplicationDbContext _context;

        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Document> GetDocumentByIdAsync(Guid id)
        {
            var document = await _context.Documents
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.DocumentId == id);

            if (document == null)
            {
                throw new KeyNotFoundException($"Document with ID {id} was not found.");
            }

            return document;
        }

        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return await _context.Documents.Include(d => d.ApplicationUser).ToListAsync();
        }

        public async Task<Document> UploadDocumentAsync(Document document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<Document> UpdateDocumentAsync(Guid id, Document document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            var existingDocument = await _context.Documents.FindAsync(id);
            if (existingDocument == null)
            {
                throw new KeyNotFoundException($"Document with ID {id} was not found.");
            }

            existingDocument.DocumentName = document.DocumentName;

            _context.Documents.Update(existingDocument);
            await _context.SaveChangesAsync();
            return existingDocument;
        }

        public async Task<bool> DeleteDocumentAsync(Guid id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return false;
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Document>> GetDocumentsByIdsAsync(List<Guid> documentIds)
        {
            return await _context.Documents
                                 .Where(d => documentIds.Contains(d.DocumentId))
                                 .Include(d => d.ApplicationUser) 
                                 .ToListAsync();
        }

    }
}
