using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.DataModels
{
    public class Supply
    {
        public int SupplyId { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        public DateTime SupplyDate { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        public int GoodCount { get; set; }

        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        public int GoodId { get; set; }

        public Good? Good { get; set; }
    }
}
