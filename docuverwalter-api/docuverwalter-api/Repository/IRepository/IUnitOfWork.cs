namespace docuverwalter_api.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IDocumentRepository Document { get; }
        void Save();
    }
}
