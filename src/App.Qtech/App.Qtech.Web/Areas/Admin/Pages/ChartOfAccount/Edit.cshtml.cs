using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IRoleModuleAccessService _roleModuleAccessService;

        public EditModel(ILogger<EditModel> logger,IChartOfAccountService chartOfAccountService, IRoleModuleAccessService roleModuleAccessService)
        {
            _logger = logger;
            _chartOfAccountService = chartOfAccountService;
            _roleModuleAccessService = roleModuleAccessService;
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;

        public List<App.Qtech.Domain.Entities.ChartOfAccount> ParentAccounts { get; set; } = new List<App.Qtech.Domain.Entities.ChartOfAccount>();

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
                    _logger.LogWarning("User does not have access to Update RoleModuleAccess.");
                    return RedirectToPage("./AccessDenied");
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
                var parentAccounts = await _chartOfAccountService.GetAllAccountsWithoutChildAsync(id);
                if (chartofaccount == null)
                {
                    return NotFound();
                }
                ChartOfAccount = chartofaccount;
                ParentAccounts = parentAccounts;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve account");
                return RedirectToPage("./Error");
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(await _chartOfAccountService.IsDuplicate(ChartOfAccount.Name))
                    {
                        _logger.LogWarning("Duplicate Account");
                        TempData["ErrorMessage"] = "An account with this name already exists";
                        return RedirectToPage("./Index");
                    }
                    await _chartOfAccountService.UpdateAccountAsync(ChartOfAccount);
                    TempData["SuccessMessage"] = "Account updated successfully";
                    return Page();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Faild to update account");
                }
            }
            try
            {
                ParentAccounts = await _chartOfAccountService.GetAllAccountsWithoutChildAsync(ChartOfAccount.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve parent accounts");
                return RedirectToPage("./Error");
            }
            return Page();
        }
    }
}
