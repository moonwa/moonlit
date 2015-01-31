using System.ComponentModel.DataAnnotations;

namespace Moonlit.Data.Fixtures.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; } 
    }
}
 