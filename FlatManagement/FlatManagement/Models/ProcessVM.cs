using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class ProcessVM
    {
        [Key]
        public int Id { get; set; }
        public int ClaimRefId { get; set; }



        [DisplayName("Flow Type")]
        [Column(TypeName = "nvarchar(50)")]
        public string Flow { get; set; }

        public string curr_ApprovedByRole { get; set; }
        public string Next_ApprovedByRole { get; set; }

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


        [DisplayName("Expense Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TransDate { get; set; } = DateTime.Now;

        [DisplayName("CurrentDate")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; } = DateTime.Now;

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
        [Column(TypeName = "nvarchar(256)")]
        public string FlatOwner { get; set; }

        [DisplayName("Payment Status")]
        [Column(TypeName = "nvarchar(256)")]
        public string PaymentStatus { get; set; }

        public bool DeleteFlag { get; set; }


        [DisplayName("Current Status")]
        [Column(TypeName = "nvarchar(50)")]
        public string CurrentStatus { get; set; } //current status of transaction like= submitted, Close, paid etc
        public string TransactionCategory { get; set; }

        /// <summary>
        /// 0 - initial
        /// 1- user can claim
        /// 2-user claimed
        /// 3-approved by treasury
        /// 4-split amount and sent to admin
        /// 5-admin split the amount and sent to all users as Bill
        /// </summary>
        [DisplayName("Claim")]
        public int Claim { get; set; }//flat owner can claim only when the flow type is expense and final paid by treasurer etc


        [DisplayName("Split Amount")]
        public double SplitAmt { get; set; }


        [DisplayName("Receipt File")]
        [Column(TypeName = "nvarchar(50)")]
        public string ReceiptFile { get; set; }






        //ForBill
        [DisplayName("Bill Type")]
        [Column(TypeName = "nvarchar(50)")]
        public string? BillType { get; set; }

        [DisplayName("Bill Date")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BillDate { get; set; }

        [DisplayName("Bill For")]
        public string BillFor { get; set; }

        [DisplayName("Bill Amount")]
        public double BillAmount { get; set; }


        [DisplayName("Bill Due Date")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BillDueDate { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; } = DateTime.Now;


        //[DisplayName("Type")]
        //public FlowType FlowType { get; set; }
        [DisplayName("Type")]
        public TransType FlowTypes { get; set; }
    }

    public enum TransType
    {
        [Display(Name = "Expenses")]
        Expenses,
        [Display(Name = "Receive")]
        Receive,
        [Display(Name = "Advance")]
        Advance        
    }
}

