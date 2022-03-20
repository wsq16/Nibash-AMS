using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class ServiceVM
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Type")]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This field is required.")]
        public string Type { get; set; }
        
        [DisplayName("Amount")]
        [Required(ErrorMessage = "This field is required.")]
        public double Amount { get; set; }

        [Required]
        [DisplayName("Start Date")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [DisplayName("End Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "End Date field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; } = DateTime.Today;
        [DisplayName("Details")]
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "This field is required.")]
        public string Details { get; set; }
        [DisplayName("Bill Type")]
        [Column(TypeName = "nvarchar(55)")]
        [Required(ErrorMessage = "This field is required.")]
        public string BillType { get; set; }

        [DisplayName("File-1")]
        [Column(TypeName = "nvarchar(56)")]
        public string FileUpload1 { get; set; }
        [DisplayName("File-2")]
        [Column(TypeName = "nvarchar(56)")]
        public string FileUpload2 { get; set; }
        [DisplayName("File-3")]
        [Column(TypeName = "nvarchar(56)")]
        public string FileUpload3 { get; set; }

        [DisplayName("Prepaired By")]
        [Column(TypeName = "nvarchar(256)")]
        [Required(ErrorMessage = "This field is required.")]
        public string PrepairedBy { get; set; }


        [DisplayName("Status")]
        [Column(TypeName = "nvarchar(56)")]
        public string Status { get; set; }


        [DisplayName("Status:")]
        public bool isActive { get; set; }

        [DisplayName("Code")]
        public string ApartCodeName { get; set; }
    }
}
