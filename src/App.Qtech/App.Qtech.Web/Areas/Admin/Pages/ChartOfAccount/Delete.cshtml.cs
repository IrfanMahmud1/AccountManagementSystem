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
    public class DeleteModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IRoleModuleAccessService _roleModuleAccessService;

        public DeleteModel(ILogger<EditModel> logger, IChartOfAccountService chartOfAccountService, IRoleModuleAccessService roleModuleAccessService)
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
                if (!await _roleModuleAccessService.CanAcessAsync(role, "ChartOfAccount", "Delete"))
                {
                    _logger.LogWarning("User does not have access to Delete RoleModuleAccess.");
                    return RedirectToPage("/AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            if (Guid.Empty == id)
            {
                return NotFound();
            }

            try
            {
                var chartofaccount = await _chartOfAccountService.GetAccountByIdAsync(id);
                if (chartofaccount == null)
                {
                    return NotFound();
                }
                if(await _chartOfAccountService.IsParentAccount(id))
                {
                    _logger.LogWarning("Cannot delete account with child accounts.");
                    TempData["ErrorMessage"] = "Cannot delete account with child accounts. Please delete child accounts first.";
                    return RedirectToPage("./Index");
                }
                else
                    ChartOfAccount = chartofaccount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve account");
                return RedirectToPage("./Error");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (Guid.Empty == id)
            {
                return NotFound();
            }

            try
            {
                await _chartOfAccountService.DeleteAccountAsync(id);
                TempData["SuccessMessage"] = "Account deleted successfully";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Faild to delete account");
            }

            return Page();
        }
    }
}
