using EsignPlatform.BLL.Mapper;
using EsignPlatform.BLL.Services.Implementations;
using EsignPlatform.BLL.Services.Interfaces;
using EsignPlatform.DAL.Data;
using EsignPlatform.DAL.Entities;
using EsignPlatform.DAL.Repositories;
using EsignPlatform.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

namespace EsignPlatform.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            // QuestPDF community license (pulsuz layihələr üçün)
            QuestPDF.Settings.License = LicenseType.Community;

            builder.Services.AddControllersWithViews();

            // ---- Database (DAL) ----
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ---- Identity ----
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(o =>
            {
                o.LoginPath = "/Account/Login";
                o.AccessDeniedPath = "/Account/AccessDenied";
                o.ExpireTimeSpan = TimeSpan.FromHours(8);
            });

            builder.Services.AddMemoryCache();
            builder.Services.AddAutoMapper(cfg =>
     cfg.AddMaps(typeof(MappingProfile).Assembly));

            // ---- Repositories (DAL) ----
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
            builder.Services.AddScoped<IContractRepository, ContractRepository>();

            // ---- Services (BLL) ----
            builder.Services.AddScoped<ITemplateService, TemplateService>();
            builder.Services.AddScoped<IContractService, ContractService>();
            builder.Services.AddScoped<ISignatureService, SignatureService>();
            builder.Services.AddScoped<IOtpService, OtpService>();
            builder.Services.AddScoped<IPdfService, PdfService>();

            var app = builder.Build();

            // ---- Migrate + Seed ----
            using (var scope = app.Services.CreateScope())
            {
                var sp = scope.ServiceProvider;
                var context = sp.GetRequiredService<AppDbContext>();
                var userManager = sp.GetRequiredService<UserManager<AppUser>>();
                var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
                await DbInitializer.SeedAsync(context, userManager, roleManager);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
