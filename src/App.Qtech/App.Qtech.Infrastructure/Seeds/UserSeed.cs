using App.Qtech.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace App.Qtech.Infrastructure.Seeds
{
    public static class UserSeed
    {
        public static async Task SeedUserAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                                        .CreateLogger("UserSeed");

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // Admin user
            string adminRoleName = "Admin";
            string adminEmail = "admin@app-qtech.com";
            string adminPassword = "app-qtech201Admin";

            // Accountant user
            string accountantRoleName = "Accountant";
            string accountantEmail = "accountant@app-qtech.com";
            string accountantPassword = "app-qtech201Accountant";

            try
            {
                // Ensure roles exist
                foreach (var roleName in new[] { adminRoleName, accountantRoleName })
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        var role = new ApplicationRole
                        {
                            Name = roleName,
                            NormalizedName = roleName.ToUpper()
                        };

                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                                logger.LogError($"Error creating role");
                            return;
                        }
                    }
                }

                // Create Admin user
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        Name = "System Admin",
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                    };

                    var createResult = await userManager.CreateAsync(user, adminPassword);
                    if (createResult.Succeeded)
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(user, adminRoleName);
                        if (!addToRoleResult.Succeeded)
                        {
                            foreach (var error in addToRoleResult.Errors)
                                logger.LogError($"Error adding admin to role");
                        }
                        else
                        {
                            logger.LogInformation("Admin user created and assigned role.");
                        }
                    }
                    else
                    {
                        foreach (var error in createResult.Errors)
                            logger.LogError($"Error creating admin user");
                    }
                }
                else
                {
                    logger.LogInformation("Admin user already exists.");
                }

                // Create Accountant user
                var accountantUser = await userManager.FindByEmailAsync(accountantEmail);
                if (accountantUser == null)
                {
                    var user = new ApplicationUser
                    {
                        Name = "Accountant",
                        UserName = accountantEmail,
                        Email = accountantEmail,
                        EmailConfirmed = true,
                    };

                    var createResult = await userManager.CreateAsync(user, accountantPassword);
                    if (createResult.Succeeded)
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(user, accountantRoleName);
                        if (!addToRoleResult.Succeeded)
                        {
                            foreach (var error in addToRoleResult.Errors)
                                logger.LogError($"Error adding accountant to role");
                        }
                        else
                        {
                            logger.LogInformation("Accountant user created and assigned role.");
                        }
                    }
                    else
                    {
                        foreach (var error in createResult.Errors)
                            logger.LogError($"Error creating accountant user");
                    }
                }
                else
                {
                    logger.LogInformation("Accountant user already exists.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the users.");
            }
        }
    }
}
