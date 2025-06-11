using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;
using App.Qtech.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using App.Qtech.Application.Services;
using System.Security.Claims;

namespace App.Qtech.Web.Areas.Admin.Pages.ChartOfAccount
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly ILogger<CreateModel> _logger;
        private readonly IRoleModuleAccessService _roleModuleAccessService;

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;
        public List<App.Qtech.Domain.Entities.ChartOfAccount> ParentAccounts { get; set; } = new List<App.Qtech.Domain.Entities.ChartOfAccount>();
        public CreateModel(IChartOfAccountService chartOfAccountService, ILogger<CreateModel> logger, IRoleModuleAccessService roleModuleAccessService)
        {
            _chartOfAccountService = chartOfAccountService;
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
                if (!await _roleModuleAccessService.CanAcessAsync(role, "ChartOfAccount", "Create"))
                {
                    _logger.LogWarning("User does not have access to Create RoleModuleAccess.");
                    return RedirectToPage("/AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            try
            {
                ParentAccounts = await _chartOfAccountService.GetAllAccountsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve parent accounts");
                return RedirectToPage("./Error");
            }
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _chartOfAccountService.AddAccountAsync(ChartOfAccount);
                    TempData["SuccessMessage"] = "Account created successfully";
                    return RedirectToPage("./Index");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Faild to create account");
                }
            }
            try
            {
                ParentAccounts = await _chartOfAccountService.GetAllAccountsAsync();
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
