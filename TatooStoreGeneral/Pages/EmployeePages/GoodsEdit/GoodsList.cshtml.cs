using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models;
using TatooStore.Models.DataModels;
using TatooStore.Services.Repository.AccountRepository;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages.EmployeePages.GoodsEdit
{
    [Authorize(Roles = RoleList.employeeRole)]
    public class GoodsListModel : PageModel
    {
        private readonly IGoodsRepository _repos;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountRepository _account;

        [BindProperty]
        public List<Good> Goods { get; set; }

        public GoodsListModel(IGoodsRepository repos, UserManager<AppUser> userManager, IAccountRepository account)
        {
            _repos = repos;
            _userManager = userManager;
            _account = account;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Goods = await _repos.GetGoodsPartiallyAsync(20, null, null);
            return Page();
        }
        public async Task<IActionResult> OnPostAddSupplyAsync(int goodId, int goodCount)
        {
            Supply supply = new Supply()
            {
                GoodId = goodId,
                GoodCount = goodCount,
                SupplyDate = DateTime.Now,
                Employee = await _account.GetEmployeeAsync(_userManager.FindByNameAsync(User.Identity.Name).GetAwaiter().GetResult().Id),
            };
            await _repos.AddSupplyAsync(supply);
            await OnGetAsync();
            return Page();
        }
    }
}
