using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models;

namespace TatooStoreGeneral.Pages.CustomerPages
{
    [Authorize(Roles = RoleList.customerRole)]
    public class AboutUsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
