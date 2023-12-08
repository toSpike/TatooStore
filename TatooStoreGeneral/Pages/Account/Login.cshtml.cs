using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels.Account;
using TatooStore.Services.Repository.AccountRepository;

namespace TatooStoreGeneral.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAccountRepository _repos;

        [BindProperty]
        public Login Model { get; set; }
        public LoginModel(SignInManager<AppUser> signInManager, IAccountRepository repos)
        {
            _signInManager = signInManager;
            _repos = repos;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string ?returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (await _repos.IsActiveUserAsync(Model.Email))
                {
                    var identityResult = await _signInManager.PasswordSignInAsync(Model.Email, Model.Password, Model.RememberMe, false);
                    if (identityResult.Succeeded)
                    {
                        if (returnUrl == null || returnUrl == "/")
                        {
                            return RedirectToPage("/Index");
                        }
                        else
                        {
                            return RedirectToPage(returnUrl);
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                }
                ModelState.AddModelError(string.Empty, "Отказано в доступе");
            }

            return Page();
        }
    }
}
