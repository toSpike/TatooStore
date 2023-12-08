using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.ViewModels
{
    public class GoodVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Cost
        {
            get
            {
                if (Count != null && Price != null)
                {
                    return Price * Count;
                } else
                {
                    return 0m;
                }
            }
        }
    }
    public class BasketVM
    {
        public string? deliveryAddress { get; set; }
        public decimal TotalCost
        {
            get
            {
                if (Goods != null)
                {
                    decimal total = 0;
                    foreach (var item in Goods)
                    {
                        total += item.Cost;
                    }
                    return total;
                } else
                {
                    return 0m;
                }
            }
        }
        public List<GoodVM> Goods { get; set; } = new List<GoodVM> { };


    }
}
