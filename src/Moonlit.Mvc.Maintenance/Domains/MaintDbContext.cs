using System.Data.Entity;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Domains
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))] 
    public class MaintDbContext : DbContext
    {
        static MaintDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MaintDbContext, Migrations.Configuration>());
        }
         
        public MaintDbContext()
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(x => x.Roles).WithMany().Map(x=>x.MapLeftKey("UserId").MapRightKey("RoleId"));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Culture> Cultures { get; set; }
        public DbSet<CultureText> CultureTexts { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
    }
}