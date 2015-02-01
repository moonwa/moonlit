using System.Data.Entity;

namespace AssemblyV1
{
    public class DbContextV1 : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Cat> Cats { get; set; }
        public DbSet<Dog> Dogs { get; set; }
    }
}