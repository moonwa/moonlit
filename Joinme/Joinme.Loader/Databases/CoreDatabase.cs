
using System.Data.Entity;

namespace Joinme.Loader.Databases
{
    public class CoreDatabase : DbContext
    {
        public CoreDatabase():base("Name")
        {
            Database.SetInitializer<CoreDatabase>(null);
        }    
    }
}
