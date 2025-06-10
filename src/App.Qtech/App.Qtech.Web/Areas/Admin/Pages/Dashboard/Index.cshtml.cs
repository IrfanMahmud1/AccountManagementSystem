using App.Qtech.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace App.Qtech.Web.Areas.Admin.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRoleModuleAccessService _roleModuleAccessService;

        public IndexModel(ILogger<IndexModel> logger, IRoleModuleAccessService roleModuleAccessService)
        {
            _logger = logger;
            _roleModuleAccessService = roleModuleAccessService;
        }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == null)
                {
                    _logger.LogWarning("User role not found in claims.");
                    return RedirectToPage("./Index");
                }
                if (!await _roleModuleAccessService.CanAcessAsync(role, "Dashboard", "View"))
                {
                    _logger.LogWarning("User does not have access to View Dashboard.");
                    RedirectToPage("./AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            _logger.LogInformation("Dashboard accessed at {Time}", DateTime.UtcNow);
            return Page();
        }
    }
}
