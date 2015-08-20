using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class MaintDbContextMaintDbRepository : IMaintDbRepository
    {
        public DbSet<SystemJob> SystemJobs
        {
            get { return _database.SystemJobs; }
        }

        public DbSet<Culture> Cultures
        {
            get { return _database.Cultures; }
        }

        public DbSet<CultureText> CultureTexts
        {
            get { return _database.CultureTexts; }
        }

        public DbSet<SystemSetting> SystemSettings
        {
            get { return _database.SystemSettings; }
        }

        public DbSet<User> Users
        {
            get { return _database.Users; }
        }

        public DbSet<Role> Roles
        {
            get { return _database.Roles; }
        }

        public DbSet<ExceptionLog> ExceptionLogs
        {
            get { return _database.ExceptionLogs; }
        }

       

        public void SaveChanges()
        {
            _database.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _database.SaveChangesAsync().ConfigureAwait(false);
        }
         
        MaintDbContext _database;

        public MaintDbContextMaintDbRepository()
            : this(new MaintDbContext())
        {

        }
        public MaintDbContextMaintDbRepository(MaintDbContext database)
        {
            _database = database;
        }

        public void Dispose()
        {
            using (_database)
            {

            }
            _database = null;
        }
    }
}