using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels.Account.Registration;
using TatooStore.Services.Repository.AccountRepository;

namespace TatooStoreGeneral.Pages.Account.Registration
{
    [Authorize(Roles = RoleList.adminRole)]
    public class EmployeeRegistrationModel : PageModel
    {
        private readonly IAccountRepository _repos;

        [BindProperty]
        public EmployeeRegistration Model { get; set; }
        public EmployeeRegistrationModel(IAccountRepository repos)
        {
            _repos = repos;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var employeeResult = await _repos.CreateEmployeeAsync(Model);
                if (employeeResult.Succeeded)
                {
                    // Добавить путь возврата
                    return RedirectToPage("/EmployeePages/EmployeesList/EmployeesList");
                }
                foreach (var error in employeeResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
        public void OnGet()
        {
        }
    }
}
