using System.Data.Entity;
using Moonlit.Data.Migrations;

namespace Moonlit.Data.Fixtures.Models
{
    public class TestDbContext : DbContext
    { 
        public TestDbContext()
            : base(TestHelper.ConnectionString)
        {
        }

        static TestDbContext()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseInitializer<TestDbContext>());
        }
     
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().ToTable("Persons");
        }
        public DbSet<Person> Persons { get; set; }
    }
}

