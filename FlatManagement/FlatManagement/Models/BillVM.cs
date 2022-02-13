using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class BillVM
    {
        [Key]
        public int Id { get; set; }
       
        [DisplayName("Type")]
        [Required(ErrorMessage ="This field is required.")]
        [Column(TypeName="nvarchar(250)")]
        public string? BillType { get; set; }


        [Required(ErrorMessage ="This field is required.")]
        [DisplayName("Bill Date")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BillDate { get; set; }


        [Required(ErrorMessage ="This field is required.")]
        [DisplayName("Bill For")]
        [Column(TypeName = "nvarchar(250)")]
        public string BillFor { get; set; }


        [DisplayName("Flat No")]
        [Column(TypeName = "varchar(250)")]
        public string FlatNo { get; set; }

        [DisplayName("Amount")]
        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "This field is required.")]
        public double Amount { get; set; }

        [Required]
        [Column(TypeName = "DateTime")]
        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        //[Required]
        //[Column(TypeName = "DateTime")]
        //[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        //public Nullable<System.DateTime> BillPaidDate { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string Remarks { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string PreparedBy { get; set; }
        
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EntryDate { get; set; }

         
        [DisplayName("Bill Status")]
        [Column(TypeName = "varchar(25)")]
        public string BillStatus { get; set; }

        public CartStatus Status { get; set; }
        public enum CartStatus
        {
            Owners = 1,
            Common = 2,
            Others = 3
        }

        //public IList<SelectListItem> BillItems { get; set; }

    }
}
