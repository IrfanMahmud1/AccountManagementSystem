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
using App.Qtech.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using App.Qtech.Application.Services;
using System.Security.Claims;

namespace App.Qtech.Web.Areas.Admin.Pages.ChartOfAccount
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ILogger<DetailsModel> _logger;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IRoleModuleAccessService _roleModuleAccessService;

        public DetailsModel(ILogger<DetailsModel> logger, IChartOfAccountService chartOfAccountService, IRoleModuleAccessService roleModuleAccessService)
        {
            _logger = logger;
            _chartOfAccountService = chartOfAccountService;
            _roleModuleAccessService = roleModuleAccessService;
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == null)
                {
                    _logger.LogWarning("User role not found in claims.");
                    return RedirectToPage("./Index");
                }
                if (!await _roleModuleAccessService.CanAcessAsync(role, "ChartOfAccount", "Update"))
                {
                    _logger.LogWarning("User does not have access to details.");
                    RedirectToPage("./AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            if (Guid.Empty == id )
            {
                return NotFound();
            }

            try
            {
                ChartOfAccount = await _chartOfAccountService.GetHierarchyAsync(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve child account details");
                return RedirectToPage("./Error");
            }

            return Page();
        }
    }
}
