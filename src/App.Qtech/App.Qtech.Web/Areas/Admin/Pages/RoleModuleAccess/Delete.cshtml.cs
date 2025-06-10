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
using System.Security.Claims;

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class DeleteModel : PageModel
    {
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        private readonly ILogger<DeleteModel> _logger;

        [BindProperty]
        public App.Qtech.Domain.Entities.RoleModuleAccess RoleModuleAccess { get; set; } = default!;
        public DeleteModel(IRoleModuleAccessService roleModuleAccessService, ILogger<DeleteModel> logger)
        {
            _roleModuleAccessService = roleModuleAccessService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role == null)
                {
                    _logger.LogWarning("User role not found in claims.");
                    return RedirectToPage("./Index");
                }
                if (!await _roleModuleAccessService.CanAcessAsync(role, "RoleModuleAccess", "Create"))
                {
                    _logger.LogWarning("User does not have access to Create RoleModuleAccess.");
                    RedirectToPage("/AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }

            if (id == Guid.Empty)
            {
                return NotFound();
            }

            try
            {
                RoleModuleAccess = await _roleModuleAccessService.GetRoleModuleAccessByIdAsync(id);
                if (RoleModuleAccess == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve RoleModuleAccess for deletion");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            try
            {
                await _roleModuleAccessService.RemoveRoleModuleAccessAsync(id);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete RoleModuleAccess");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the Role Module Access. Please try again.");
            }

            return Page();
        }
    }
}
