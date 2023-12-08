using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TatooStoreGeneral.Pages
{
    public class ResultModel : PageModel
    {
        public string Title { get; set; }
        public void OnGet(string? answer)
        {
            Title = answer;
        }
        public IActionResult OnPostOk()
        {
            return RedirectToPage("/Index");
        }
    }
}
