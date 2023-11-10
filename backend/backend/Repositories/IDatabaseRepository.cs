using backend.Entities;
using Backend.Filters;

namespace backend.Repositories
{
    public interface IDatabaseRepository<T> where T : IDocument
    {
        void Add(T document);
        void Clear();
        T FindById(string id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(
            int? limit,
            SortBy? sortBy = SortBy.Time,
            SortOrder sortOrder = SortOrder.Ascending,

            IEnumerable<string>? sensorTypes = null,
            IEnumerable<int>? instances = null,
            DateTime? startTime = null,
            DateTime? endTime = null);

        IEnumerable<T> Get(Sort sort, Filters filters);
    }
}