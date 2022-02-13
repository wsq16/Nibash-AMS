using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.ViewModel
{
    public class TransactionBillViewModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Purpose")]
        [Required(ErrorMessage = "This field is required.")]
        public string Purpose { get; set; }

        [DisplayName("Paid By")]
        public string PaidBy { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "This field is required.")]
        public double Amount { get; set; }

        [DisplayName("Split Amount")]
        [Required(ErrorMessage = "This field is required.")]
        public double SplitAmount { get; set; }
       
        public int BillId { get; set; }

        [DisplayName("Date")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DisplayName("Prepared By")]
        public string PreparedBy { get; set; }

        [DisplayName("Receipt Number")]
        public string ReceiptNumber { get; set; }

        [DisplayName("Flat Number")]
        public string FlatNo { get; set; }

        [DisplayName("Flat Owner")]
        public string FlatOwner { get; set; }

        [DisplayName("Status")]
        public string TransactionStep { get; set; } //current status of transaction like= submitted, Close, etc

        [DisplayName("Claim")]
        public int Claim { get; set; }

        [DisplayName("Receipt File")]
        public string ReceiptFile { get; set; }

        public string TransactionCategory { get; set; }



        [DisplayName("Next Status")]
        public string NextStatus { get; set; }
        public string TransactionTypes { get; set; }


        [DisplayName("Type")]
        [Required(ErrorMessage = "This field is required.")]
        public string? BillType { get; set; }


        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Bill Date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BillDate { get; set; }


        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Bill For")]
        public string BillFor { get; set; }


        [Required]
        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }


        public string Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EntryDate { get; set; }


        [DisplayName("Bill Status")]
        public string BillStatus { get; set; }

       

    }
}
