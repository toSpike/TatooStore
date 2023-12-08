using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.DataModels
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(512)]
        public string DeliveryAddress { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        public IList<PurchaseItem>? PurchaseItems { get; set; }
    }
}
