using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatooStore.Models.DataModels;

namespace TatooStore.Models.ViewModels.EmployeePages.GoodsEdit.Create
{
    public class CreateGoodVM
    {
        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(128)]
        public string GoodName { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(1024)]
        public string GoodDescription { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        public int GoodCount { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Range(1, 100), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [MaxLength(256)]
        public string? GoodImagePath { get; set; }

        public int Manufactures { get; set; }

        public int Categories { get; set; }
    }
}
