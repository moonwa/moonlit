using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyV1
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        public string StringNullableField { get; set; }
        [StringLength(30)]
        [Required]
        public string StringNotNullableField { get; set; }
        public int? IntNullableField { get; set; }
        public int IntNotNullableField { get; set; }
        public decimal? DecimalNullableField { get; set; }
        public decimal DecimalNotNullableField { get; set; }
        public bool? BooleanNullableField { get; set; }
        public bool BooleanNotNullableField { get; set; }
    }
}
