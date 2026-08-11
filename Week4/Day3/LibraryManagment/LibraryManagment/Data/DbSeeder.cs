using System;
using LibraryManagment.Enum;
using Microsoft.AspNetCore.Identity;


namespace LibraryManagment.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await SeedRoles(roleManager);
            await SeedUser(userManager, "userAdmin@gmail.com", "Admin1234@", UserRole.Admin);
            await SeedUser(userManager, "userCustomer@gmail.com", "Customer1234@", UserRole.Customer);

        }

        private static async Task SeedUser(UserManager<IdentityUser> userManager, string email, string password, UserRole role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
                return;
            var user2 = new IdentityUser { UserName = email, Email = email };
            var result = await userManager.CreateAsync(user2, password);
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user2, role.ToString());
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (UserRole role in System.Enum.GetValues(typeof(UserRole)))
            {
                var roleName = role.ToString();
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

            }
        }
    }
}
