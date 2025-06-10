using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class CreateModel : PageModel
    {
        private readonly App.Qtech.Infrastructure.Data.ApplicationDbContext _context;

        public CreateModel(App.Qtech.Infrastructure.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.RoleModuleAccess RoleModuleAccess { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RoleModuleAccesses.Add(RoleModuleAccess);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
