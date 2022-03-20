using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class ResolutionVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Agenda")]
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string AgendaName { get; set; }

        [DisplayName("Point No")]
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string PointNo { get; set; }

        [DisplayName("Resolution Note")]
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string Resolution { get; set; }


        [DisplayName("Closing Note")]
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(256)")]
        public string ResolutionClosingNote { get; set; }


        [DisplayName("Responsibility(Flat Owner)")]
        [Column(TypeName = "nvarchar(45)")]
        public string ResponsibilityFlatOwner { get; set; }
        
        [DisplayName("Responsibility(Employee)")]
        [Column(TypeName = "nvarchar(45)")]
        public string ResponsibilityEmployee { get; set; }

        [DisplayName("Responsibility(Employee)")]
        [Column(TypeName = "nvarchar(45)")]
        public string ResponsibilityEmployeeName { get; set; }

        [DisplayName("Start Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "Date field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; } = DateTime.Now;

        [DisplayName("Due Date")]
        [Column(TypeName = "DateTime")]
        [Required(ErrorMessage = "Date field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; } = DateTime.Now;

        [DisplayName("Start Time")]
        [Column(TypeName = "nvarchar(10)")]
        public string startTime { get; set; } = "00:00";

        [DisplayName("End Time")]
        [Column(TypeName = "nvarchar(10)")]
        public string endTime { get; set; } = "00:00";


        //[DisplayName("Status")]
        //[Column(TypeName = "nvarchar(25)")]
        //[Required(ErrorMessage = "Status field is required.")]
        //public string Status { get; set; }

        [DisplayName("Attachment")]
        [Column(TypeName = "nvarchar(55)")]
        public string Attachment { get; set; }



        public bool msg_SMS { get; set; }
        public bool msg_EMAIL { get; set; }
        public bool msg_BOTH { get; set; }

        public bool to_Committee { get; set; }
        public bool to_Treasurer { get; set; }
        public bool to_FlatOwner { get; set; }
        public bool to_All { get; set; }


        [DisplayName("Status")]
        [Column(TypeName = "nvarchar(45)")]
        [MaxLength(45)]
        public string Status { get; set; }

        //[DisplayName("Status")]
        //[Column(TypeName = "nvarchar(45)")]
        //[MaxLength(45)]
        //[EnumDataType(typeof(ResolutionStatus))]
        //public ResolutionStatus Status { get; set; }

        [DisplayName("Code")]
        public string ApartCodeName { get; set; }


    }

    public enum ResolutionStatus
    {
        Open,
        InProgress,
        Completed
    }
}
