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

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class EditModel : PageModel
    {
        private readonly App.Qtech.Infrastructure.Data.ApplicationDbContext _context;

        public EditModel(App.Qtech.Infrastructure.Data.ApplicationDbContext context)
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

            var rolemoduleaccess =  await _context.RoleModuleAccesses.FirstOrDefaultAsync(m => m.Id == id);
            if (rolemoduleaccess == null)
            {
                return NotFound();
            }
            RoleModuleAccess = rolemoduleaccess;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RoleModuleAccess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleModuleAccessExists(RoleModuleAccess.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RoleModuleAccessExists(Guid id)
        {
            return _context.RoleModuleAccesses.Any(e => e.Id == id);
        }
    }
}
