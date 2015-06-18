using System;
using System.Linq;
using System.Threading.Tasks;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public interface IMaintDbRepository : IDisposable
    {
        IQueryable<Culture> Cultures { get; }
        IQueryable<CultureText> CultureTexts { get; }
        IQueryable<SystemSetting> SystemSettings { get; }
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<ExceptionLog> ExceptionLogs{ get; }
        void Add(Culture culture);
        void Add(CultureText cultureText);
        void Add(SystemSetting systemSetting);
        void Add(User user);
        void Add(Role role);
        void SaveChanges();
        Task SaveChangesAsync();
        void Remove(CultureText cultureText);
        void Add(ExceptionLog exceptionLog);
    }
}