using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Invoice.Domain.Entities;
using System.Threading.Tasks;

namespace Invoice.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 🌟 Seed Roles
            var roles = new[] { "ADMIN", "USER" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 🌟 Seed Admin User
            var adminEmail = "admin@invoice.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123"); // change to secure password
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "ADMIN");
                }
            }
        }
    }
}
