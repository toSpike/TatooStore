using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Services.Repository.AccountRepository;
using TatooStore.Models.DataModels;
using Microsoft.AspNetCore.Authorization;
using TatooStore.Models;

namespace TatooStoreGeneral.Pages.EmployeePages.EmployeesList
{
    [Authorize(Roles = RoleList.adminRole)]
    public class EmployeesListModel : PageModel
    {
        private readonly IAccountRepository _repos;
        public List<Employee> employee { get; set; }

        public EmployeesListModel(IAccountRepository repos)
        {
            _repos = repos;
        }
        public async Task OnGetAsync()
        {
            employee = await _repos.GetAllEmployeeAsync();
        }
        public async Task OnPostDeleteEmployeeAsync(string id)
        {
            await _repos.DeleteEmployeeAsync(id);
            employee = await _repos.GetAllEmployeeAsync();
        }
    }
}
