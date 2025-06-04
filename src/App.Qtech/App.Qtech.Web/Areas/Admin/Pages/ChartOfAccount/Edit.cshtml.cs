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

namespace App.Qtech.Web.Areas.Admin.Pages.ChartOfAccount
{
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly App.Qtech.Domain.Services.IChartOfAccountService _chartOfAccountService;

        public EditModel(ILogger<EditModel> logger, Domain.Services.IChartOfAccountService chartOfAccountService)
        {
            _logger = logger;
            _chartOfAccountService = chartOfAccountService;
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;

        public List<App.Qtech.Domain.Entities.ChartOfAccount> ParentAccounts { get; set; } = new List<App.Qtech.Domain.Entities.ChartOfAccount>();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
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
                    await _chartOfAccountService.UpdateAccountAsync(ChartOfAccount);

                    return RedirectToPage("./Index");
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
