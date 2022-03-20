using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class SupplierVM
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }
        [DisplayName("Amount")]
        [Required(ErrorMessage = "This field is required.")]
        public double Amount { get; set; }
        [DisplayName("Prepared By")]
        [Column(TypeName = "nvarchar(25)")]
        public string PrepairedBy { get; set; }

        [DisplayName("Start Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime Start_Date { get; set; } = DateTime.Today;

        [DisplayName("End Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime End_Date { get; set; } = DateTime.Today;

        [DisplayName("Per-Address")]
        [Column(TypeName = "nvarchar(250)")]
        public string Per_Address { get; set; }

        [DisplayName("Pre-Address")]
        [Column(TypeName = "nvarchar(250)")]
        public string Pre_Address	 { get; set; }

       
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email")]
        [Column(TypeName = "nvarchar(25)")]
        public string Email { get; set; }
        
        [DisplayName("Mobile")]
        [Column(TypeName = "nvarchar(25)")]
        public string Mobile { get; set; }
        [DisplayName("NID")]
        [Column(TypeName = "nvarchar(25)")]
        public string NID { get; set; }
        [DisplayName("E-TIN")]
        [Column(TypeName = "nvarchar(25)")]
        public string ETIN { get; set; }

        [DisplayName("Code")]
        public string ApartCodeName { get; set; }




    }
}
