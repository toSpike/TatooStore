using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models.DataModels;

namespace TatooStoreGeneral.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LogoutModel(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }
        public IActionResult OnPostDontLogoutAsync()
        {
            return RedirectToPage("/Index");
        }
    }
}
