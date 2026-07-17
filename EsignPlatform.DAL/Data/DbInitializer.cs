using EsignPlatform.DAL.Entities;
using EsignPlatform.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Data
{
    public static class DbInitializer
    {
        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        public static async Task SeedAsync(
            AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            // Roles
            foreach (var role in new[] { AdminRole, UserRole })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Admin user
            const string adminEmail = "admin@smartcontract.az";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    UserType = UserType.Individual,
                    FinOrVoen = "0000000",
                    DisplayName = "System Admin"
                };
                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, AdminRole);
            }

            // Templates
            if (!await context.Templates.AnyAsync())
            {
                context.Templates.AddRange(
                    new Template
                    {
                        Name = "Kirayə müqaviləsi",
                        Category = TemplateCategory.Rental,
                        Description = "Daşınmaz əmlakın kirayəyə verilməsi üçün standart müqavilə.",
                        SchemaJson = """
                    [
                      { "key": "propertyAddress", "label": "Əmlakın ünvanı", "type": "text", "required": true },
                      { "key": "rentAmount", "label": "Aylıq kirayə məbləği (AZN)", "type": "number", "required": true },
                      { "key": "durationMonths", "label": "Müddət (ay)", "type": "number", "required": true },
                      { "key": "startDate", "label": "Başlama tarixi", "type": "date", "required": true }
                    ]
                    """
                    },
                    new Template
                    {
                        Name = "Xidmət müqaviləsi",
                        Category = TemplateCategory.Service,
                        Description = "Xidmətin göstərilməsi üzrə müqavilə.",
                        SchemaJson = """
                    [
                      { "key": "serviceName", "label": "Xidmətin adı", "type": "text", "required": true },
                      { "key": "amount", "label": "Xidmətin dəyəri (AZN)", "type": "number", "required": true },
                      { "key": "deadline", "label": "Son tarix", "type": "date", "required": true },
                      { "key": "details", "label": "Əlavə şərtlər", "type": "textarea", "required": false }
                    ]
                    """
                    },
                    new Template
                    {
                        Name = "Satış müqaviləsi",
                        Category = TemplateCategory.Sale,
                        Description = "Malın alqı-satqısı üzrə müqavilə.",
                        SchemaJson = """
                    [
                      { "key": "itemName", "label": "Malın adı", "type": "text", "required": true },
                      { "key": "price", "label": "Qiymət (AZN)", "type": "number", "required": true },
                      { "key": "quantity", "label": "Miqdar", "type": "number", "required": true }
                    ]
                    """
                    },
                    new Template
                    {
                        Name = "Borc müqaviləsi",
                        Category = TemplateCategory.Debt,
                        Description = "Pul vəsaitinin borc verilməsi üzrə müqavilə.",
                        SchemaJson = """
                    [
                      { "key": "amount", "label": "Borc məbləği (AZN)", "type": "number", "required": true },
                      { "key": "returnDate", "label": "Qaytarılma tarixi", "type": "date", "required": true },
                      { "key": "interest", "label": "Faiz (%)", "type": "number", "required": false }
                    ]
                    """
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }

}
