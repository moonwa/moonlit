using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public interface IMaintDbRepository : IDisposable
    {
        DbSet<SystemJob> SystemJobs { get; }
        DbSet<Culture> Cultures { get; }
        DbSet<CultureText> CultureTexts { get; }
        DbSet<SystemSetting> SystemSettings { get; }
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<ExceptionLog> ExceptionLogs { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}