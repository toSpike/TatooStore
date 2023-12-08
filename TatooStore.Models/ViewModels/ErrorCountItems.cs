using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatooStore.Models.ViewModels
{
    public class ErrorCountItems
    {
        public string Title { get; set; }
        public List<ErrorBasketVm> Errors { get; set; } = new List<ErrorBasketVm>();

    }
}
