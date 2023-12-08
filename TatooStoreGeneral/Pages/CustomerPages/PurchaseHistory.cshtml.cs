using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models;
using TatooStore.Models.ViewModels.CustomerPages;
using TatooStore.Services.Repository.AccountRepository;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages.CustomerPages
{
    [Authorize(Roles = RoleList.customerRole)]
    public class PurchaseHistoryModel : PageModel
    {
        private readonly IGoodsRepository _repos;
        private readonly IAccountRepository _account;

        public PurchaseHistoryModel(IGoodsRepository repos, IAccountRepository account)
        {
            _repos = repos;
            _account = account;
        }
        public PurchaseHistoryVM PurchaseHistory { get; set; } = new PurchaseHistoryVM();
        public void OnGet()
        {
            PurchaseHistory = _repos.GetPurchaseHistoryAsync(_account.GetCustomerIdAsync(User.Identity.Name).GetAwaiter().GetResult()).GetAwaiter().GetResult();
        }
    }
}
