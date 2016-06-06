using System.Data.Entity;
using System.Linq;

namespace Joinme.Loader
{
    // ReSharper disable once InconsistentNaming
    public class EFRepository<TEntity, TDatabase> : IRepository<TEntity>
        where TDatabase : DbContext
        where TEntity : class, IEntityObject
    {
        private readonly TDatabase _database;

        public EFRepository(TDatabase database)
        {
            _database = database;
        }

        protected DbSet<TEntity> Entities
        {
            get { return _database.Set<TEntity>(); }
        }

        public void Add(TEntity item)
        {
            Entities.Add(item);
        }

        public void Delete(TEntity item)
        {
            Entities.Remove(item);
        }

        public IQueryable<TEntity> Query()
        {
            return Entities;
        }

        public void Update(TEntity item)
        {
            Entities.Remove(item);
        }
    }
}