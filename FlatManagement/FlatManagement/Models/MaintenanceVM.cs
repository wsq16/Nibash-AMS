using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class MaintenanceVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Category field is required.")]
        [Column(TypeName = "nvarchar(250)")]
        public string category { get; set; }

        [DisplayName("Maintenance Type")]
        [Required(ErrorMessage = "Maintenance Type field is required.")]
        [Column(TypeName = "nvarchar(250)")]
        public string maintenance_type{ get; set; }

        [DisplayName("Contract")]
        [Required(ErrorMessage = "Contract field is required.")]
        [Column(TypeName = "nvarchar(250)")]
        public string contract{ get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal amount { get; set; }

        [DisplayName("Date")]
        [Required(ErrorMessage = "Date field is required.")]
        [Column(TypeName = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? MaintenanceDate { get; set; } = DateTime.Now;

        [DisplayName("Entry By")]
        [Required(ErrorMessage = "Entry By field is required.")]
        [Column(TypeName = "nvarchar(250)")]
        public string entry_by{ get; set; }


        [DisplayName("Entry Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "Entry Date field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? entry_date { get; set; }

    }
}
