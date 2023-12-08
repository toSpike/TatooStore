using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models;
using TatooStore.Models.ViewModels.EmployeePages.OrderList;
using TatooStore.Services.Repository.AccountRepository;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages.EmployeePages.OrdersList
{
    [Authorize(Roles = RoleList.employeeRole)]
    public class OrderListItemModel : PageModel
    {
        private readonly IGoodsRepository _repos;
        private readonly IAccountRepository _account;

        public OrderListItemModel(IGoodsRepository repos, IAccountRepository account)
        {
            _repos = repos;
            _account = account;
        }
        public OrderListItemVM OrderListItem { get; set; }
        public async Task<IActionResult> OnGetAsync(int purchaseId)
        {
            OrderListItem = await _repos.GetOrderListItemByIdAsync(purchaseId);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int purchaseId)
        {
            int id = await _account.GetEmployeeIdAsync(User.Identity.Name);
            await _repos.AddEmployeeToPurchaseAsync(id, purchaseId);
            return RedirectToPage("/EmployeePages/OrdersList/OrdersList");
        }
    }
}
