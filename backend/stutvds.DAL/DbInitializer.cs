using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StopStatAuth_6_0.Entities;

public static class DbInitializer
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // --- Роли ---
        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // --- Администратор ---
        var adminEmail = "admin@local";
        var adminPassword = "Admin123!";
        
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser()
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
        
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (!result.Succeeded)
                throw new Exception("Не удалось создать администратора: " +
                                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        
        // --- Добавляем админа в роль ---
        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");
    }
}