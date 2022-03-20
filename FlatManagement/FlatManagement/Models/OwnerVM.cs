using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class OwnerVM
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("First Name")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string LastName { get; set; }
        [DisplayName("Email")]
        [Column(TypeName = "nvarchar(25)")]
        public string Email { get; set; }
        [DisplayName("Mobile")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string Mobile { get; set; }
        [DisplayName("DOB")]
        [Column(TypeName = "DateTime")]
        public DateTime DOB { get; set; }
        [DisplayName("NID")]
        [Column(TypeName = "nvarchar(25)")]
        [Required(ErrorMessage = "This field is required.")]
        public string NID { get; set; }
        [DisplayName("E-TIN")]
        [Column(TypeName = "nvarchar(25)")]
        public string ETIN { get; set; }
        [DisplayName("Passport")]
        [Column(TypeName = "nvarchar(25)")]
        public string PassportNo { get; set; }
        [DisplayName("Per-Address")]
        [Column(TypeName = "nvarchar(250)")]
        public string Per_Address { get; set; }
        [DisplayName("Pre-Address")]
        [Column(TypeName = "nvarchar(250)")]
        public string pre_Address { get; set; }

        [DisplayName("Flat No")]
        [Column(TypeName = "nvarchar(250)")]
        public string Flat_No { get; set; }

        [DisplayName("User Role")]
        [Column(TypeName = "nvarchar(250)")]
        public string UserRole { get; set; }



        [DisplayName("Tenant")]
        public bool Tenant { get; set; }

        [DisplayName("Code")]
        public string ApartCodeName { get; set; }

    }
}
