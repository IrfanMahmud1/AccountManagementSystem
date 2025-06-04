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

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chartofaccount =  await _context.ChartOfAccounts.FirstOrDefaultAsync(m => m.Id == id);
            if (chartofaccount == null)
            {
                return NotFound();
            }
            ChartOfAccount = chartofaccount;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(App.Qtech.Domain.Entities.ChartOfAccount model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _chartOfAccountService.UpdateAccountAsync(model);

                    return RedirectToPage("./Index");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Faild to update account");
                }
            }
            ParentAccounts = await _chartOfAccountService.GetAllAccountsAsync();
            return Page();
        }

        private bool ChartOfAccountExists(Guid id)
        {
            return _context.ChartOfAccounts.Any(e => e.Id == id);
        }
    }
}
