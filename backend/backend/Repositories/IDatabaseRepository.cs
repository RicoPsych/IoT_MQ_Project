using backend.Entities;

namespace backend.Repositories
{
    public interface IDatabaseRepository<T> where T : IDocument
    {
        void Add(T document);
        T FindById(string id);
        IEnumerable<T> GetAll();
    }
}