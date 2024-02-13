using docuverwalter_api.Models;

namespace docuverwalter_api.Repository.IRepository
{
    public interface IDocumentRepository : IRepository<Document>
    {
        void Update(Document document);
    }
}
