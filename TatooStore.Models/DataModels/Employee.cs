using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.DataModels
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public DateTime DateOfEmployment { get; set; }

        [Required]
        [StringLength(10)]
        public string PassportNumber { get; set; }

        public string UserId { get; set; }
        public AppUser? User { get; set; }

        public IList<Supply>? Supplies { get; set; }

        public IList<Purchase>? Purchases { get; set; }
    }
}
