using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;
using App.Qtech.Domain.Services;

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class DeleteModel : PageModel
    {
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        private readonly ILogger<IndexModel> _logger;

        public DeleteModel(IRoleModuleAccessService roleModuleAccessService, ILogger<IndexModel> logger)
        {
            _roleModuleAccessService = roleModuleAccessService;
            _logger = logger;
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
