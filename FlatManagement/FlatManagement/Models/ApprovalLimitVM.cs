using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class ApprovalLimitVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "nvarchar(65)")]
        public string? RoleName { get; set; }

        public string Flow { get; set; }



        [DisplayName("Amount Limit(Min)")]
        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public double AmountLimit_MIN { get; set; }

        [DisplayName("Amount Limit(Max)")]
        [Required(ErrorMessage = "This field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public double AmountLimit_MAX { get; set; }

        [Display(Name = "Active")]
        public bool isActive { get; set; }

        [Required]
        [Column(TypeName = "DateTime")]
        [Display(Name = "Create Date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;

        //public IEnumerable<SelectListItem> RolesList { get; set; }
        public string PreparedBy { get; set; }

        [Display(Name = "Flow")]
        public FlowType FlowTypes { get; set; }

        [DisplayName("Code")]
        public string ApartCodeName { get; set; }

    }

    

    public enum FlowType
    {
        [Display(Name = "Expenses")]
        Expenses,
        [Display(Name = "Receive")]
        Receive,
        [Display(Name = "Advance")]
        Advance,
        [Display(Name = "Claim")]
        Claim
    }



}
