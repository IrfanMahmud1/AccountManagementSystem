using App.Qtech.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Qtech.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRoleModuleAccessService _roleModuleAccessService;

        [BindProperty(SupportsGet = true)]
        public List<string> Modules { get; set; } = new List<string>();

        public IndexModel(ILogger<IndexModel> logger, IRoleModuleAccessService roleModuleAccessService)
        {
            _logger = logger;
            _roleModuleAccessService = roleModuleAccessService;
        }

        public async Task OnGet()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                Modules = await _roleModuleAccessService.GetAllModuleNamesByRoleNameAsync(role);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve modules for the user role.");
            }
        }
    }
}
