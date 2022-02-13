using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class AgendaVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Name")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Agenda Name field is required.")]
        public string AgendaName { get; set; }


        [DisplayName("Meeting Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "Date field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AgendaDate { get; set; }


        [DisplayName("Location")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string LocationStr { get; set; }


        [DisplayName("Agenda Details")]
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(256)")]
        [Required(ErrorMessage = "Agenda Detail field is required.")]
        public string AgendaDetails { get; set; }
        

        [DisplayName("Attachment")]
        [Column(TypeName = "nvarchar(50)")]
        public string Attachment { get; set; }
    }
}
