using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.ViewModels.CustomerPages
{
    public class PurchaseHistoryVM
    {
        public List<PurchaseVM> Purchases { get; set; } = new List<PurchaseVM>();
    }
    public class PurchaseVM
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<PurchaseItemVM> PurchaseItems { get; set; } = new List<PurchaseItemVM>();
        public bool Status { get; set; }
        public decimal TotalCost { get; set; } = 0;
    }
    public class PurchaseItemVM
    {
        public int PurchaseItemId { get; set; }
        public int GoodCount { get; set; }
        public decimal GoodPrice { get; set; }
        public int GoodId { get; set; }
        public string GoodName { get; set; }
    }
}
