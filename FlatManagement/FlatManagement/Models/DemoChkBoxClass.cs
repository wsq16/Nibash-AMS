using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class DemoChkBoxClass
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string DemoVal { get; set; }
        [NotMapped]
        public bool checkboxAnswer { get; set; }

    }
}
