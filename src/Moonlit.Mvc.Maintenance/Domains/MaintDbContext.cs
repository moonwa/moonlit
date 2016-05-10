using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

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
            modelBuilder.Entity<User>().HasMany(x => x.Roles).WithMany().Map(x => x.MapLeftKey("UserId").MapRightKey("RoleId"));
            modelBuilder.Entity<User>().HasMany(x => x.LoginFailedLogs).WithRequired(x => x.User).HasForeignKey(x => x.UserId);
            base.OnModelCreating(modelBuilder);
        }
        public static Func<int?> GetCurrentUser = null;
        private static int? GetOperatorId()
        {
            var getCurrentUser = GetCurrentUser;
            if (getCurrentUser != null)
            {
                return getCurrentUser();
            }
            return null;
        }
        public override int SaveChanges()
        {
            var userId = GetOperatorId();
            foreach (DbEntityEntry<IAuditObject> tradeLog in ChangeTracker.Entries<IAuditObject>())
            {
                if (tradeLog.State == EntityState.Added)
                {
                    tradeLog.Entity.CreationUserId = userId;
                    tradeLog.Entity.CreationTime = DateTime.Now;
                }
                if (tradeLog.State == EntityState.Modified)
                {
                    tradeLog.Entity.UpdateUserId = userId;
                    tradeLog.Entity.UpdateTime = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        public DbSet<UserLoginFailedLog> UserLoginFailedLogs { get; set; }
        public DbSet<SystemJob> SystemJobs { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<CultureText> CultureTexts { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
    }
}