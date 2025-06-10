using App.Qtech.Domain.Entities;
using App.Qtech.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Qtech.Infrastructure.Seeds
{
    public static class RoleModuleAccessSeed
    {
        public static async Task SeedUserAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                                        .CreateLogger("UserSeed");
            var roleModuleAccessService = serviceProvider.GetRequiredService<IRoleModuleAccessService>();
            var adminRoleName = "Admin";
            try
            {
                foreach (var module in new[] { "ChartOfAccount", "Voucher", "RoleModuleAccess" })
                {
                    foreach (var operation in new[] { "Create", "Edit", "Delete", "View" })
                    {
                        if (!await roleModuleAccessService.CanAcessAsync(adminRoleName, module, operation))
                        {
                            await roleModuleAccessService.CreateRoleModuleAccessAsync(new RoleModuleAccess
                            {
                                RoleName = adminRoleName,
                                ModuleName = module,
                                Operation = operation
                            });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed to seed RoleModuleAccess");
            }
        }
    }
}
