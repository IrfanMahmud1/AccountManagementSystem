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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using App.Qtech.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        private readonly ILogger<CreateModel> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;

        [BindProperty]
        public App.Qtech.Domain.Entities.RoleModuleAccess RoleModuleAccess { get; set; } = default!;

        public List<string> Roles { get; set; } = new List<string>();
        public List<string> Modules { get; set; } = new List<string>();
        public List<string> Operations { get; set; } = new List<string>();
        public CreateModel(IRoleModuleAccessService roleModuleAccessService,
            ILogger<CreateModel> logger, RoleManager<ApplicationRole> roleManager)
        {
            _roleModuleAccessService = roleModuleAccessService;
            _logger = logger;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGet()
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
                    return RedirectToPage("/AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                if (roles.Count == 0)
                {
                    _logger.LogWarning("User does not have any roles assigned.");
                    return RedirectToPage("/AccessDenied");
                }
                foreach (var role in roles)
                {
                    Roles.Add(role.Name);
                }
                Modules = new List<string> { "ChartOfAccount", "Voucher", "RoleModuleAccess" }; // Replace with actual module retrieval logic
                Operations = new List<string> { "View", "Create", "Update", "Delete" }; // Replace with actual module retrieval logic
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve roles for RoleModuleAccess creation.");
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
                    var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    if(await _roleModuleAccessService.CanAcessAsync(role, "RoleModuleAccess", "Create"))
                    {
                        _logger.LogWarning("User already have access to create RoleModuleAccess.");
                        TempData["ErrorMessage"] = "You already have access to create Role Module Access.";
                        return RedirectToPage("./Index");
                    }
                    await _roleModuleAccessService.CreateRoleModuleAccessAsync(RoleModuleAccess);
                    TempData["SuccessMessage"] = "Role Module Access created successfully.";
                    return RedirectToPage("./Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create RoleModuleAccess");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the Role Module Access. Please try again.");
                }
            }
                
            return Page();
        }
    }
}
