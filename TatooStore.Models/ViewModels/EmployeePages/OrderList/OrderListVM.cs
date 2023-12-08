using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.ViewModels.EmployeePages.OrderList
{
    public class OrderListVM
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int? EmployeeId { get; set; }
        public decimal TotalCost { get; set; } = 0;
        public int CustomerId { get; set; }
    }
}
