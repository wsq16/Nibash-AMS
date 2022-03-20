using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class FlatConfigVM
    {
        [Key]
        public int Id { get; set; }

        public int slId { get; set; }

        public bool isWing { get; set; }
       
        //public int Id { get; set; }

        //[DisplayName("Wing")]
        //[Column(TypeName = "nvarchar(25)")]
        //[Required(ErrorMessage = "Wing field is required.")]
        public int? Wing { get; set; }

        //[DisplayName("Total Wing")]
        //[Column(TypeName = "nvarchar(25)")]
        //[Required(ErrorMessage = "Total Wing field is required.")]
        public int? TotalWing { get; set; }

        //[DisplayName("Ground Floor")]
        //[Column(TypeName = "nvarchar(25)")]
        //[Required(ErrorMessage = "Wing field is required.")]
        public int? GroundFloorStart { get; set; }


        //[DisplayName("Wing Per Floor")]
        public int? WingPerFloor { get; set; }

        //[DisplayName("Flat Per Wing")]
        //[Required(ErrorMessage = "Flat Per Wing field is required.")]
        public int? FlatPerWing { get; set; }

        //[DisplayName("Floor")]
        //[Column(TypeName = "nvarchar(25)")]
        //[Required(ErrorMessage = "Floor field is required.")]
        public int? Floor { get; set; }

        
        [DisplayName("Flat No")]
        [Column(TypeName = "nvarchar(25)")]
       // [Required(ErrorMessage = "Flat No field is required.")]
        public string FlatNo { get; set; }


        [Column(TypeName = "nvarchar(10)")]
        public string WingName { get; set; }

        //[DisplayName("Total Flat")]
        //[Column(TypeName = "decimal(18)")]
       // [Required(ErrorMessage = "Total Floor field is required.")]
        public int? TotalFlat { get; set; }

        //[DisplayName("Flat Per Floor")]
        //[Column(TypeName = "decimal(18)")]
        //[Required(ErrorMessage = "Flat Per Floor field is required.")]
        public int? FlatPerFloor { get; set; }

        //[DisplayName("Flat Start From")]
        //[Column(TypeName = "decimal(18)")]
        //[Required(ErrorMessage = "This field is required.")]
        public int? FlatStartFrom { get; set; }


        //[DisplayName("Sequence")]
        //[Column(TypeName = "decimal(5)")]
        public int? Sequence { get; set; }

        //[DisplayName("Delimeter")]
        [Column(TypeName = "nvarchar(25)")]
        public string Delimeter { get; set; }

        [DisplayName("Code")]
        public string ApartCodeName { get; set; }
    }
}
