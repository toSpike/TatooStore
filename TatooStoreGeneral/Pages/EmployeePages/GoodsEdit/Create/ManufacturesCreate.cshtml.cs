using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models;
using TatooStore.Models.DataModels;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages.EmployeePages.GoodsEdit.Create
{
    [Authorize(Roles = RoleList.employeeRole)]
    public class ManufacturesCreateModel : PageModel
    {
        private readonly IGoodsRepository _repos; 

        [BindProperty]
        public Manufacture Manufacture { get; set; }

        public ManufacturesCreateModel(IGoodsRepository repos)
        {
            _repos = repos;
        }
        public async Task<IActionResult> OnPost()
        {
            var result = await _repos.AddManufactureAsync(Manufacture);
            if (result > 0)
            {
                return RedirectToPage("/EmployeePages/GoodsEdit/GoodsList");
            }
            ModelState.AddModelError(string.Empty, "Ошибка при добавлении производителя");
            return Page();
        }
        public void OnGet()
        {
        }
    }
}
