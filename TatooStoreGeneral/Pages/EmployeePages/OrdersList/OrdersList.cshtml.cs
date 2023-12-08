using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models;
using TatooStore.Models.ViewModels.EmployeePages.OrderList;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages.EmployeePages.OrdersList
{
    [Authorize(Roles = RoleList.employeeRole)]
    public class OrdersListModel : PageModel
    {
        private readonly IGoodsRepository _repos;

        public OrdersListModel(IGoodsRepository repos)
        {
            _repos = repos;
        }
        public List<OrderListVM> OrderList { get; set; }
        public void OnGet()
        {
            OrderList = _repos.GetOrderListAsync().GetAwaiter().GetResult();
        }
    }
}
