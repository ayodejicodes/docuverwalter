using docuverwalter_api.Data;
using docuverwalter_api.Models;
using docuverwalter_api.Repository.IRepository;
using System.Linq.Expressions;

namespace docuverwalter_api.Repository
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Document document)
        {
            _context.Documents.Update(document);
        }
    }
}
