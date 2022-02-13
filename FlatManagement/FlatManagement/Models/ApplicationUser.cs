using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string City { get; set; }
       
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        
       
        public string Mobile { get; set; }
       
        public string NID { get; set; }
       
        public string ETIN { get; set; }
       

        public string PassportNo { get; set; }        
        public string Per_Address { get; set; }        
        public string pre_Address { get; set; }
        public string Flat_No { get; set; }
        public string UserRole { get; set; }

        public bool Tenant { get; set; }
        public bool IsActive { get; set; }

        public string FlatOwner { get; set; }


    }
}
