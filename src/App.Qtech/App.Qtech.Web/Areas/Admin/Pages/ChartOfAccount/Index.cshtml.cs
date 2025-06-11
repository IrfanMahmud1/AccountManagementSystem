using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;
using App.Qtech.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using App.Qtech.Application.Services;
using System.Security.Claims;

namespace App.Qtech.Web.Areas.Admin.Pages.ChartOfAccount
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly ILogger<IndexModel> _logger;
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        public IndexModel(IChartOfAccountService chartOfAccountService, ILogger<IndexModel> logger, IRoleModuleAccessService roleModuleAccessService)
        {
            _chartOfAccountService = chartOfAccountService;
            _logger = logger;
            _roleModuleAccessService = roleModuleAccessService;
        }

        public IList<App.Qtech.Domain.Entities.ChartOfAccount> ChartOfAccount { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == null)
                {
                    _logger.LogWarning("User role not found in claims.");
                    return RedirectToPage("./Index");
                }
                if (!await _roleModuleAccessService.CanAcessAsync(role, "ChartOfAccount", "View"))
                {
                    _logger.LogWarning("User does not have access to View RoleModuleAccess.");
                    return RedirectToPage("./AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            try
            {
                ChartOfAccount = await _chartOfAccountService.GetAllAccountsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve parent accounts");
            }
            return Page();
        }
    }
}
