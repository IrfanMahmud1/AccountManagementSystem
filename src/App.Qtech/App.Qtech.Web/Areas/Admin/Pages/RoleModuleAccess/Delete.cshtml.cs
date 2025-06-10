using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class DeleteModel : PageModel
    {
        private readonly App.Qtech.Infrastructure.Data.ApplicationDbContext _context;

        public DeleteModel(App.Qtech.Infrastructure.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public App.Qtech.Domain.Entities.RoleModuleAccess RoleModuleAccess { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolemoduleaccess = await _context.RoleModuleAccesses.FirstOrDefaultAsync(m => m.Id == id);

            if (rolemoduleaccess is not null)
            {
                RoleModuleAccess = rolemoduleaccess;

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

            var rolemoduleaccess = await _context.RoleModuleAccesses.FindAsync(id);
            if (rolemoduleaccess != null)
            {
                RoleModuleAccess = rolemoduleaccess;
                _context.RoleModuleAccesses.Remove(RoleModuleAccess);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
