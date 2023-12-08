using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.ViewModels.EmployeePages.OrderList
{
    public class OrderListItemVM
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int? EmployeeId { get; set; }
        public decimal TotalCost { get; set; } = 0;
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItemVM> OrderItems { get; set; } = new List<OrderItemVM>();  
    }
    public class OrderItemVM
    {
        public int PurchaseItemId { get; set; }
        public int GoodCount { get; set; }
        public decimal GoodPrice { get; set; }
        public int GoodId { get; set; }
        public string GoodName { get; set; }
    }
}
