using GymManagementSystem.DAL.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Data.Seeder
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration)
        {


            if (await roleManager.RoleExistsAsync(IdentityRoleNames.SuperAdmin) == false)
            {
                var superAdminRole = new ApplicationRole
                {
                    Name = IdentityRoleNames.SuperAdmin,
                    DisplayName = "Super Administrator",
                    Id = Guid.NewGuid()
                };

                var result = roleManager.CreateAsync(superAdminRole);
                if (!result.Result.Succeeded)
                {
                    throw new Exception("Failed to create SuperAdmin role");
                }
            }



            if (await roleManager.RoleExistsAsync(IdentityRoleNames.Admin) == false)
            {
                var adminRole = new ApplicationRole
                {
                    Name = IdentityRoleNames.Admin,
                    DisplayName = "Administrator",
                    Id = Guid.NewGuid()
                };
                var result = await roleManager.CreateAsync(adminRole);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create Admin role");
                }
            }


            var superAdminEmail = configuration["IdentitySeed:SuperAdmin:Email"] ?? "superadmin@gym.com";
            var superAdminPassword = configuration["IdentitySeed:SuperAdmin:Password"] ?? "SuperAdmin@123";

            if (await userManager.FindByEmailAsync(superAdminEmail) == null)
            {
                var superAdminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    FullName = "Main Super Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()

                };

                var createResult = await userManager.CreateAsync(superAdminUser, superAdminPassword);

                if (!await userManager.IsInRoleAsync(superAdminUser, IdentityRoleNames.SuperAdmin) && createResult.Succeeded)
                {

                    await userManager.AddToRoleAsync(superAdminUser, IdentityRoleNames.SuperAdmin);

                    if (!createResult.Succeeded)
                    {
                        throw new Exception("Failed to add SuperAdmin to role");
                    }
                }
            }


            var adminEmail = configuration["IdentitySeed:Admin:Email"] ?? "admin@gym.com";
            var adminPassword = configuration["IdentitySeed:Admin:Password"] ?? "Admin@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Gym Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()

                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);



                if (!await userManager.IsInRoleAsync(adminUser, IdentityRoleNames.Admin) && createResult.Succeeded)
                {

                    await userManager.AddToRoleAsync(adminUser, IdentityRoleNames.Admin);

                    if (!createResult.Succeeded)
                    {
                        throw new Exception("Failed to add Admin to role");
                    }
                }

            }

        }
    }
}
