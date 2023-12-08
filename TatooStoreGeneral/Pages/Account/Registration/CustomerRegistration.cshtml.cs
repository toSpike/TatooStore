using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels.Account.Registration;
using TatooStore.Services.Repository.AccountRepository;

namespace TatooStoreGeneral.Pages.Account.Registration
{
    [AllowAnonymous]
    public class CustomerRegistrationModel : PageModel
    {
        private readonly IAccountRepository _repos;

        [BindProperty]
        public CustomerRegistration Model { get; set; }
        public CustomerRegistrationModel(IAccountRepository repos)
        {
            _repos = repos;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var customerResult = await _repos.CreateCustomerAsync(Model);
                if (customerResult.Succeeded)
                {
                    // Добавить путь возврата
                    return RedirectToPage("/Index");
                }
                foreach (var error in customerResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
