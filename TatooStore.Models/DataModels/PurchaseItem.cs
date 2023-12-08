﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.DataModels
{
    public class PurchaseItem
    {
        public int PurchaseItemId { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        public int GoodCount { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Range(1, 1000000000), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal GoodPrice { get; set; }

        public int PurchaseId { get; set; }
        public Purchase? Purchase { get; set; }

        public int GoodId { get; set; }

        public Good? Good { get; set; }
    }
}
