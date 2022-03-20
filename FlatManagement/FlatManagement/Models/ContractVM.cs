using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class ContractVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Contract Name")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string ContractName { get; set; }
        [DisplayName("Contact Person")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string ContactPerson { get; set; }

        [DisplayName("Start Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Description")]
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "This field is required.")]
        public string Description { get; set; }

        [DisplayName("Bill Type")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string BillType { get; set; }

        [DisplayName("Bill Frequency")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string BillFrequency { get; set; }

        [DisplayName("Code")]
        public string ApartCodeName { get; set; }
    }                 
}
