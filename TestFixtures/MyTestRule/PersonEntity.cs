using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestRule
{
    public class EntityBase
    {
        public int Id { get; set; }
    }
    public class PersonEntity : EntityBase
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
    }
}
