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
    public class EmployeeVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //[Display(Name = "Full Name")]
        //public string FullName
        //{
        //    get { return FirstName + " " + LastName; }
        //}

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Email")]
        public string Email { get; set; }

       
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("NID")]
        public string EmpNID { get; set; }

        

        [Required]
        [Column(TypeName = "varchar(25)")]
        public string Mobile { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Type { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Status { get; set; }

        [Required]
        [DisplayName("Joining Date")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JoiningDate { get; set; }

        [Column(TypeName = "varchar(256)")]        
        public string EntryBy { get; set; }

        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EntryDate { get; set; }

        [DisplayName("Image")]
        [Column(TypeName = "nvarchar(150)")]
        public string PicLoc { get; set; }

        //[Column(TypeName = "nvarchar(100)")]
        //[DisplayName("Image Name")]
        //public string ImageName { get; set; }

        //[NotMapped]
        //[DisplayName("Upload File")]
        //public IFormFile ImageFile { get; set; }

        //[Display(Name = "Full Name")]
        //public string FullName { get; set; }

        //public EmployeeVM()
        //{
        //    FullName = FirstName + " " + LastName;
        //}
    }
}
