using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class FlatVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Flat Number")]
        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "nvarchar(250)")]
        public string FlatNo { get; set; }
        public string FlatValue { get; set; }

        [DisplayName("Remarks")]
        [Column(TypeName = "nvarchar(250)")]
        public string Remarks { get; set; }
    }
}
