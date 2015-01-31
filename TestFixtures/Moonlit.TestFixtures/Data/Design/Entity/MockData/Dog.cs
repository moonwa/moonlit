using System.ComponentModel.DataAnnotations;

namespace AssemblyV1
{
    public class Dog
    {
        [Key]
        public int DogId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}