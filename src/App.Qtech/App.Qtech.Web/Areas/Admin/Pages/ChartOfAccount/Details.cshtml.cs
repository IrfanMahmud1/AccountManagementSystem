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
    public class DetailsModel : PageModel
    {
        private readonly App.Qtech.Infrastructure.Data.ApplicationDbContext _context;

        public DetailsModel(App.Qtech.Infrastructure.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.ChartOfAccount ChartOfAccount { get; set; } = default!;
        public List<App.Qtech.Domain.Entities.ChartOfAccount> ChildAccounts { get; set; } = new List<App.Qtech.Domain.Entities.ChartOfAccount>();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (Guid.Empty == id )
            {
                return NotFound();
            }

            try
            {
                
            }

            if (chartofaccount is not null)
            {
                ChartOfAccount = chartofaccount;

                return Page();
            }

            return NotFound();
        }
    }
}
