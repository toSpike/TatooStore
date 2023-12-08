using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models.DataModels;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGoodsRepository _repos;

        public List<Category> Categories { get; set; }

        public IndexModel(IGoodsRepository repos)
        {
            _repos = repos;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _repos.GetAllCategoriesAsync();
            return Page();
        }
    }
}