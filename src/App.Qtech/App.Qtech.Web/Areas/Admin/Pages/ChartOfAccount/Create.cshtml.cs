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

namespace App.Qtech.Web.Areas.Admin.Pages.ChartOfAccount
{
    public class CreateModel : PageModel
    {
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;
        public List<App.Qtech.Domain.Entities.ChartOfAccount> ParentAccounts { get; set; } = new List<App.Qtech.Domain.Entities.ChartOfAccount>();
        public CreateModel(IChartOfAccountService chartOfAccountService, ILogger<CreateModel> logger)
        {
            _chartOfAccountService = chartOfAccountService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
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
