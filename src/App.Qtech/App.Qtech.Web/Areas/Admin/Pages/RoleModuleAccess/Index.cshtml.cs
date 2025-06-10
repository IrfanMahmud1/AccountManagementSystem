using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Qtech.Domain.Entities;
using App.Qtech.Infrastructure.Data;
using App.Qtech.Domain.Repositories;
using App.Qtech.Domain.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

namespace App.Qtech.Web.Areas.Admin.Pages.RoleModuleAccess
{
    public class IndexModel : PageModel
    {
        private readonly IRoleModuleAccessService _roleModuleAccessService;
        private readonly ILogger<IndexModel> _logger;   

        public IndexModel(IRoleModuleAccessService roleModuleAccessService, ILogger<IndexModel> logger)
        {
            _roleModuleAccessService = roleModuleAccessService;
            _logger = logger;
        }

        public IList<App.Qtech.Domain.Entities.RoleModuleAccess> RoleModuleAccess { get;set; } = default!;

        public async Task OnGetAsync()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if(role == null)
                {
                    _logger.LogWarning("User role not found in claims.");
                    return;
                }
                if(!await _roleModuleAccessService.CanAcessAsync(role, "RoleModuleAccess"))
                {
                    _logger.LogWarning("User does not have access to view RoleModuleAccess.");
                    RedirectToPage("/AccessDenied");
                }
                RoleModuleAccess = await _roleModuleAccessService.GetAllRoleModuleAccessesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve RoleModuleAccesses");
            }
        }
    }
}
