using System.Linq;
using System.Threading.Tasks;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class MaintDbContextMaintDbRepository : IMaintDbRepository
    {

        public IQueryable<Culture> Cultures
        {
            get { return _database.Cultures; }
        }

        public IQueryable<CultureText> CultureTexts
        {
            get { return _database.CultureTexts; }
        }

        public IQueryable<SystemSetting> SystemSettings
        {
            get { return _database.SystemSettings; }
        }

        public IQueryable<User> Users
        {
            get { return _database.Users; }
        }

        public IQueryable<Role> Roles
        {
            get { return _database.Roles; }
        }

        public IQueryable<ExceptionLog> ExceptionLogs
        {
            get { return _database.ExceptionLogs; }
        }

        public void Add(Culture culture)
        {
            _database.Cultures.Add(culture);
        }

        public void Add(CultureText cultureText)
        {
            _database.CultureTexts.Add(cultureText);
        }

        public void Add(SystemSetting systemSetting)
        {
            _database.SystemSettings.Add(systemSetting);
        }

        public void Add(User user)
        {
            _database.Users.Add(user);
        }

        public void Add(Role role)
        {
            _database.Roles.Add(role);
        }

        public void SaveChanges()
        {
            _database.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _database.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Remove(CultureText cultureText)
        {
            _database.CultureTexts.Remove(cultureText);
        }

        public void Add(ExceptionLog exceptionLog)
        {
            _database.ExceptionLogs.Add(exceptionLog);
        }

        MaintDbContext _database;

        public MaintDbContextMaintDbRepository()
            : this(new MaintDbContext())
        {

        }
        internal MaintDbContextMaintDbRepository(MaintDbContext database)
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