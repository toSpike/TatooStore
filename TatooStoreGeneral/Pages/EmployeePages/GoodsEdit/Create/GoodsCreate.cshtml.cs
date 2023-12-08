using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using TatooStore.Models;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels.EmployeePages.GoodsEdit.Create;
using TatooStore.Services.Repository.AccountRepository;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages.EmployeePages.GoodsEdit.Create
{
    [Authorize(Roles = RoleList.employeeRole)]
    public class GoodsCreateModel : PageModel
    {
        private readonly IGoodsRepository _repos;
        private readonly IWebHostEnvironment _appEnviroment;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountRepository _account;

        [BindProperty]
        public Good Good { get; set; }
        public List<Manufacture> Manufactures { get; set; }
        public List<Category> Categories { get; set; }
        public PriceChange PriceChange { get; set; }
        public SelectList CategoriesSl { get; set; }
        public SelectList ManufacturesSl { get; set; }
        [BindProperty]
        public Supply Supply { get; set; }

        public GoodsCreateModel(IGoodsRepository repos, IWebHostEnvironment appEnviroment, UserManager<AppUser> userManager, IAccountRepository account)
        {
            _repos = repos;
            _appEnviroment = appEnviroment;
            _userManager = userManager;
            _account = account;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Manufactures = await _repos.GetAllManufacturesAsync();
            Categories = await _repos.GetAllCategoriesAsync();
            CategoriesSl = new SelectList(Categories, "CategoryId", "CategoryName", Categories[0].CategoryId);
            ManufacturesSl = new SelectList(Manufactures, "ManufactureId", "ManufactureName", Manufactures[0].ManufactureId);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile? uploadedImage)
        {
            var culture = new CultureInfo("en-EN");
            PriceChange = new PriceChange();
            PriceChange.NewPrice = Convert.ToDecimal(Request.Form["PriceChange.NewPrice"], culture);
            PriceChange.PriceChangeDate = DateTime.Now;
            Good.PriceChanges = new List<PriceChange>();
            Good.PriceChanges.Add(PriceChange);
            ModelState.ClearValidationState(nameof(Good));
            if (!TryValidateModel(Good, nameof(Good)))
            {
                await OnGetAsync();
                return Page();
            }
            if (uploadedImage != null)
            {
                Good.GoodImagePath = await _repos.UploadImageAsync(uploadedImage, _appEnviroment.WebRootPath, "GoodsImage");
            }
            Good = await _repos.AddGoodAsync(Good);
            Supply.Good = Good;
            Supply.SupplyDate = DateTime.Now;
            Supply.Employee = await _account.GetEmployeeAsync(_userManager.FindByNameAsync(User.Identity.Name).GetAwaiter().GetResult().Id);
            var result = await _repos.AddSupplyAsync(Supply);
            return RedirectToPage("/EmployeePages/GoodsEdit/GoodsList");
        }
    }
}
