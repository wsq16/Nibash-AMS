using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class EnumValueMV
    {
        [Key]

        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        [Column(TypeName = "varchar(20)")]
        public string Value { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "varchar(20)")]
        public EnumValueType EnumValueType { get; set; }
    }



}
