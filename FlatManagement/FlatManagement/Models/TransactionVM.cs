using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class TransactionVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Purpose")]
        [Column(TypeName = "nvarchar(500)")]
        [Required(ErrorMessage = "This field is required.")]
        public string Purpose { get; set; }
        
        [DisplayName("Paid By")]
        [Column(TypeName = "nvarchar(256)")]
        public string PaidBy { get; set; }
        
        [DisplayName("Amount")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        [Required(ErrorMessage = "This field is required.")]
        public double Amount { get; set; }

        [DisplayName("Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "This field is required.")]
        //[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.Now;

        [DisplayName("Prepared By")]
        [Column(TypeName = "nvarchar(256)")]
        public string PreparedBy { get; set; }

        [DisplayName("Receipt Number")]
        [Column(TypeName = "nvarchar(100)")]
        public string ReceiptNumber { get; set; }

        [DisplayName("Flat Number")]
        [Column(TypeName = "nvarchar(35)")]
        public string FlatNo { get; set; }

        [DisplayName("Flat Owner")]
        [Column(TypeName = "nvarchar(100)")]       
        public string FlatOwner { get; set; }

        [DisplayName("Status")]
        [Column(TypeName = "nvarchar(50)")]
        public string TransactionStep { get; set; } //current status of transaction like= submitted, Close, etc

        [DisplayName("Claim")]
        public int Claim { get; set; }

        [DisplayName("Receipt File")]
        [Column(TypeName = "nvarchar(50)")]
        public string ReceiptFile { get; set; }

        public string TransactionCategory { get; set; }


        //[DisplayName("Current Status")]
        //[Column(TypeName = "nvarchar(25)")]
        //public string CurrentStatus { get; set; }

        [DisplayName("Next Status")]
        [Column(TypeName = "nvarchar(50)")]
        public string NextStatus { get; set; }


        [DisplayName("Transaction Type")]
        public TransactionTypes TransactionType { get; set; }


       
        //ForBill
        public string? BillType { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BillDate { get; set; }
        public string BillFor { get; set; }
        public double BillAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BillDueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; } = DateTime.Now;
    }


    public enum TransactionTypes { Expenses =1, Receive =2, Advance=3 };


}
