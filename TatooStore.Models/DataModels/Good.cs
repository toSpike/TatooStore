using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.DataModels
{
    public class Good
    {
        public int GoodId { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(128)]
        public string GoodName { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(1024)]
        public string GoodDescription { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        public int GoodCount { get; set; }

        [MaxLength(256)]
        public string? GoodImagePath { get; set; }

        public int ManufactureId { get; set; }
        public Manufacture? Manufacture { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<PriceChange>? PriceChanges { get; set; }

        public List<Supply>? Supplies { get; set; }

        public List<PurchaseItem>? PurchaseItems { get; set; }
    }
}
