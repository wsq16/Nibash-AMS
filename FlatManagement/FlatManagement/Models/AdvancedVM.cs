using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{

    [Table("AdvancedPaymentTbl")]
    public class AdvancedVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Person Type")]
        public string PersonType { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Email")]
        public string PersonEmail { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public Nullable<double> Amount { get; set; }
        
        [DisplayName("Year")]
        [Required(ErrorMessage = "Year field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public string Year { get; set; }

        [DisplayName("Collection Date")]
        [Required(ErrorMessage = "Collection Date field is required.")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CollectionDate { get; set; } = DateTime.Now;

        [DisplayName("CollectionBy")]
        [Required(ErrorMessage = "Collection By field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public string CollectionBy { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "Month field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public Month Month { get; set; }


        [DisplayName("Due Date")]
        [Required(ErrorMessage = "Due Date field is required.")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AdvancePaymentDueDate { get; set; } = DateTime.Now;



        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Advanced Payment Status")]
        public string PaymentStatus { get; set; }


        [DisplayName("Code")]
        public string ApartCodeName { get; set; }
    }

}
