using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class CompanyVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Company Name")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Company Name field is required.")]
        public string  CompanyName { get; set; }

        [DisplayName("Mobile")]
        [Column(TypeName = "nvarchar(250)")]
        public string Mobile { get; set; }

        [DisplayName("Email")]
        [Column(TypeName = "nvarchar(250)")]
        public string Email { get; set; }

        [DisplayName("Address")]
        [Column(TypeName = "nvarchar(250)")]
        public string Address { get; set; }

        [DisplayName("License")]
        [Column(TypeName = "nvarchar(250)")]
        public string License { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

    }
}
