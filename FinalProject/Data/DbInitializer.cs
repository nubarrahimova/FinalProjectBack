using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            string[] roles = { "Admin", "Doctor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@medzapis.com";
            var adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Main Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var doctorSeeds = new List<(string Slug, string Email, string Password, string FullName)>
            {
              ("dr-xuraman-qaribova", "xuraman@medzapis.com", "Xuraman123!", "Dr. Xuraman Qəribova"),
              ("dr-ceyran-imamaliyeva", "ceyran@medzapis.com", "Ceyran123!", "Dr. Ceyran İmaməliyeva"),
              ("dr-xanim-bakirova", "xanim@medzapis.com", "Xanim123!", "Dr. Xanım Bəkirova")

            };

            foreach (var item in doctorSeeds)
            {
                var doctor = await dbContext.Doctors.FirstOrDefaultAsync(x => x.Slug == item.Slug);

                if (doctor == null)
                {
                    continue;
                }

                var existingUser = await userManager.FindByEmailAsync(item.Email);

                if (existingUser == null)
                {
                    var doctorUser = new AppUser
                    {
                        UserName = item.Email,
                        Email = item.Email,
                        FullName = item.FullName,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(doctorUser, item.Password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(doctorUser, "Doctor");
                        existingUser = doctorUser;
                    }
                    else
                    {
                        var errors = string.Join(" | ", result.Errors.Select(x => x.Description));
                        throw new Exception("Doctor user yaradılmadı: " + errors);
                    }
                }
                else
                {
                    if (!await userManager.IsInRoleAsync(existingUser, "Doctor"))
                    {
                        await userManager.AddToRoleAsync(existingUser, "Doctor");
                    }
                }

                if (doctor.AppUserId != existingUser.Id)
                {
                    doctor.AppUserId = existingUser.Id;
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}