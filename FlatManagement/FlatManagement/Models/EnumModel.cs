using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class EnumModel
    {
        [Key]

        public int Id { get; set; }

        [Required]
        [DisplayName("Value")]
        [Column(TypeName = "varchar(20)")]
        public string Value { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Text")]
        [Column(TypeName = "varchar(25)")]
        public String EnumText { get; set; }

        public EnumValueType EnumValueType { get; set; }



    }


    public enum EnumValueType
    {
        BILL_TYPE,
        BILL_FOR,
        FUND_TYPE,
        EXPENSE,
        PAYMENT,
        MEETING,
        CONTRACT,
        EMPLOYEE_TYPE,
        EMPLOYEE_STATUS,
        BILL_FREQUENCY,
        ROLL,
        SERVICE,
        DESIGNATION,
        MONTH,
        MAINTENANCE_TYPE
    }
}
