using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class CommitteeVM
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }

        [DisplayName("Roll")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string Roll { get; set; }
        [DisplayName("Remarks")]
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "This field is required.")]
        public string Remarks { get; set; }
    }
}
