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

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class CreateModel : PageModel
    {
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public App.Qtech.Domain.Entities.RoleModuleAccess RoleModuleAccess { get; set; } = default!;

        public CreateModel(IRoleModuleAccessService roleModuleAccessService, ILogger<IndexModel> logger)
        {
            _roleModuleAccessService = roleModuleAccessService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

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
