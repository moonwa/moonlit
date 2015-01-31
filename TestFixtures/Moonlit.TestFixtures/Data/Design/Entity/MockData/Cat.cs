using System.ComponentModel.DataAnnotations;

namespace AssemblyV1
{
    public class Cat
    {

        [Key]
        public int CatId { get; set; }

        public string Name { get; set; }
    }
}