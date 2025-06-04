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
        private readonly App.Qtech.Infrastructure.Data.ApplicationDbContext _context;

        public DeleteModel(App.Qtech.Infrastructure.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chartofaccount = await _context.ChartOfAccounts.FirstOrDefaultAsync(m => m.Id == id);

            if (chartofaccount is not null)
            {
                ChartOfAccount = chartofaccount;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chartofaccount = await _context.ChartOfAccounts.FindAsync(id);
            if (chartofaccount != null)
            {
                ChartOfAccount = chartofaccount;
                _context.ChartOfAccounts.Remove(ChartOfAccount);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
