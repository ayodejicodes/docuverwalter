using docuverwalter_api.Data;
using docuverwalter_api.Repository.IRepository;

namespace docuverwalter_api.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        public IDocumentRepository Document { get; private set; }
        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            Document = new DocumentRepository(_context);
        }
        

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
