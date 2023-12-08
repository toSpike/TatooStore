using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TatooStore.Models;
using TatooStore.Models.DataModels;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages.EmployeePages.GoodsEdit
{
    [Authorize(Roles = RoleList.employeeRole)]
    public class GoodEditModel : PageModel
    {
        private readonly IGoodsRepository _repos;
        private readonly IWebHostEnvironment _appEnvironment;

        public List<Manufacture> Manufactures { get; set; }
        public List<Category> Categories { get; set; }
        public PriceChange PriceChange { get; set; }
        public SelectList CategoriesSl { get; set; }
        public SelectList ManufacturesSl { get; set; }

        [Required(ErrorMessage = "Это поле не может быть пустым")]
        [Range(1, 1000000), DataType(DataType.Currency)]
        public string CurrentPrice { get; set; }
        public string OldPrice { get; set; }

        [BindProperty]
        public Good Good { get; set; }
        public GoodEditModel(IGoodsRepository repos, IWebHostEnvironment appEnvironment)
        {
            _repos = repos;
            _appEnvironment = appEnvironment;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var culture = new CultureInfo("en-EN");
            Good = await _repos.GetGoodByIdAsync(id);
            Manufactures = await _repos.GetAllManufacturesAsync();
            Categories = await _repos.GetAllCategoriesAsync();
            CategoriesSl = new SelectList(Categories, "CategoryId", "CategoryName", Good.CategoryId);
            ManufacturesSl = new SelectList(Manufactures, "ManufactureId", "ManufactureName", Good.ManufactureId);
            CurrentPrice = Good.PriceChanges[0].NewPrice.ToString(culture);
            OldPrice = CurrentPrice;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile? uploadedImage)
        {
            var culture = new CultureInfo("en-EN");
            PriceChange = new PriceChange();
            PriceChange.NewPrice = Convert.ToDecimal(Request.Form["CurrentPrice"], culture);
            PriceChange.PriceChangeDate = DateTime.Now;
            PriceChange.GoodId = Good.GoodId;
            ModelState.ClearValidationState(nameof(Good));
            if (!TryValidateModel(Good, nameof(Good)))
            {
                await OnGetAsync(Good.GoodId);
                return Page();
            }
            if (Convert.ToDecimal(Request.Form["OldPrice"], culture) != PriceChange.NewPrice)
            {
                await _repos.AddPriceChangeAsync(PriceChange);
            }
            if (uploadedImage != null)
            {
                Good.GoodImagePath = await _repos.UploadImageAsync(uploadedImage, _appEnvironment.WebRootPath, "GoodsImage");
            }
            var result = await _repos.ChangeGoodAsync(Good);
            if (result > 0)
            {
                return RedirectToPage("/EmployeePages/GoodsEdit/GoodsList");
            }
            ModelState.AddModelError(string.Empty, "Ошибка при добавлении товара");
            await OnGetAsync(Good.GoodId);
            return Page();
        }
    }
}
