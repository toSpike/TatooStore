using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models.ViewModels.Account.Registration;
using TatooStore.Models.DataModels;
using TatooStore.Services.Repository.AccountRepository;
using Microsoft.AspNetCore.Authorization;
using TatooStore.Models;

namespace TatooStoreGeneral.Pages.EmployeePages.EmployeesList
{
    [Authorize(Roles = RoleList.adminRole)]
    public class EmployeeEditModel : PageModel
    {
        private readonly IAccountRepository _repos;
        private Employee employeeModel { get; set; }

        [BindProperty]
        public EmployeeRegistration Employee { get; set; }

        public string ID { get; set; }

        public EmployeeEditModel(IAccountRepository repos)
        {
            _repos = repos;
        }
        public async Task OnGetAsync(string id)
        {
            ID = id;
            if (id != null)
            {
                Employee = new EmployeeRegistration();
                employeeModel = await _repos.GetEmployeeAsync(id);
                Employee.FirstName = employeeModel.User.FirstName;
                Employee.MiddleName = employeeModel.User.MiddleName;
                Employee.LastName = employeeModel.User.LastName;
                Employee.PassportNumber = employeeModel.PassportNumber;
                Employee.PhoneNumber = employeeModel.User.PhoneNumber;
                Employee.Email = employeeModel.User.Email;
                Employee.Password = "";
                Employee.ConfirmPassword = "";
            }
        }
        public async Task<IActionResult> OnPostUpdateAsync(string id)
        {
            employeeModel = await _repos.GetEmployeeAsync(id);
            await _repos.UpdateEmployeeAsync(Employee, employeeModel);
            return RedirectToPage("/EmployeePages/EmployeesList/EmployeesList");
        }
        public async Task OnPostCancelAsync()
        {

        }
    }
}
