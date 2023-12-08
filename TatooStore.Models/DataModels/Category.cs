using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.DataModels
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(128)]
        public string CategoryName { get; set; }

        [MaxLength(256)]
        public string? CategoryImagePath { get; set; }

        public IList<Good>? Goods { get; set; }
    }
}
