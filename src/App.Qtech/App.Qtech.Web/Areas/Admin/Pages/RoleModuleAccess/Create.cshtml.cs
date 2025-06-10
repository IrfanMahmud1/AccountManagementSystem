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

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class CreateModel : PageModel
    {
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        private readonly ILogger<CreateModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public App.Qtech.Domain.Entities.RoleModuleAccess RoleModuleAccess { get; set; } = default!;

        public List<string> Roles { get; set; } = new List<string>();
        public List<string> Modules { get; set; } = new List<string>();
        public List<string> Operations { get; set; } = new List<string>();
        public CreateModel(IRoleModuleAccessService roleModuleAccessService,
            ILogger<CreateModel> logger,UserManager<ApplicationUser> userManager)
        {
            _roleModuleAccessService = roleModuleAccessService;
            _logger = logger;
            _userManager = userManager;
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
                    RedirectToPage("/AccessDenied");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check user access for RoleModuleAccess creation.");
            }
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.len == 0)
                {
                    _logger.LogWarning("User does not have any roles assigned.");
                    return RedirectToPage("/AccessDenied");
                }
                Roles = roles.ToList();
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
