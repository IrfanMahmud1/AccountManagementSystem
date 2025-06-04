using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;

namespace App.Qtech.Web.Areas.Admin.Pages.ChartOfAccount
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly App.Qtech.Domain.Services.IChartOfAccountService _chartOfAccountService;

        public DeleteModel(ILogger<EditModel> logger, Domain.Services.IChartOfAccountService chartOfAccountService)
        {
            _logger = logger;
            _chartOfAccountService = chartOfAccountService;
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
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
