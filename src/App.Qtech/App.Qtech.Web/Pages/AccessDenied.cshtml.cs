using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Qtech.Web.Areas.Admin.Pages
{
    [Authorize]
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
