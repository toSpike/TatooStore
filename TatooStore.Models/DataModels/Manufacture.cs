using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.DataModels
{
    public class Manufacture
    {
        public int ManufactureId { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(128)]
        public string ManufactureName { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [MaxLength(1024)]
        public string ManufactureDescription { get; set; }

        public IList<Good>? Goods { get; set; }
    }
}
