using System.Linq;

namespace Joinme
{
    public interface IRepository<T> where T : class, IEntityObject
    {
        IQueryable<T> Query();
        void Add(T item);
        void Delete(T item);
        void Update(T item);
    }
}