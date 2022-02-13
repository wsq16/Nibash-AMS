using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class DelegatePersonHistory
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Delegate Type")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This field is required.")]
        public string DelegateType { get; set; }


        [Column(TypeName = "nvarchar(256)")]
        public string AssignByUsername { get; set; }


        [Column(TypeName = "nvarchar(256)")]
        public string AssignToUsername { get; set; }

        public int DelegetePersonId { get; set; }

        [Required]
        [DisplayName("Start Date")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime AssignResponsiblityDate { get; set; } = DateTime.Today;

        public bool IsActive { get; set; }
    }
}
