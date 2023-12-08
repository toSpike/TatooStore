using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models;
using TatooStore.Models.DataModels;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages.EmployeePages.GoodsEdit.Create
{
    [Authorize(Roles = RoleList.employeeRole)]
    public class CategoriesCreateModel : PageModel
    {
        private readonly IWebHostEnvironment _appEnviroment;
        private readonly IGoodsRepository _repos;

        [BindProperty]
        public Category Category { get; set; }

        public CategoriesCreateModel(IWebHostEnvironment appEnviroment, IGoodsRepository repos)
        {
            _appEnviroment = appEnviroment;
            _repos = repos;
        }
        public async Task<IActionResult> OnPostAsync(IFormFile uploadedImage)
        {
            if (uploadedImage != null)
            {
                Category.CategoryImagePath = await _repos.UploadImageAsync(uploadedImage, _appEnviroment.WebRootPath, "CategoriesImage");
            }
            var result = await _repos.AddCategoryAsync(Category);
            if (result > 0)
            {
                return RedirectToPage("/EmployeePages/GoodsEdit/GoodsList");
            }
            ModelState.AddModelError(string.Empty, "Категория не добавлена");
            return Page();
        }
        public void OnGet()
        {
        }
    }
}
