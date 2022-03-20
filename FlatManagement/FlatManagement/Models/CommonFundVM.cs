using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class CommonFundVM
    {
        [Key]
        public int Id { get; set; }


        [DisplayName("Fund Type")]
        [Required(ErrorMessage = "Fund Type field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public string FundType{ get; set; }

        [DisplayName("Flat No")]
        [Required(ErrorMessage = "Flat No field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public string FlatNo { get; set; }

        [DisplayName("Flat Owner")]
        [Required(ErrorMessage = "Flat Owner field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public string FlatOwner { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public Nullable<double>  Amount { get; set; }
        //public Nullable<decimal> Amount { get; set; }


        [DisplayName("Year")]
        [Required(ErrorMessage = "Year field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public string Year { get; set; }

        [DisplayName("Collection Date")]
        [Required(ErrorMessage = "Collection Date field is required.")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CollectionDate { get; set; } = DateTime.Now;

        [DisplayName("Due Date")]
        [Required(ErrorMessage = "Due Date field is required.")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CommonFundDueDate { get; set; } = DateTime.Now;
        
        [DisplayName("CollectionBy")]
        [Required(ErrorMessage = "Collection By field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public string CollectionBy { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "Month field is required.")]
        [Column(TypeName = "nvarchar(25)")]
        public Month Month { get; set; }

        [DisplayName("Code")]
        public string ApartCodeName { get; set; }
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
