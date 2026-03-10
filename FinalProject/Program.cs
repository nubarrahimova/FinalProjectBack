using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FinalProject;

public class Program
{
    public static async Task Main(string[] args)
    {
        var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("az"),
    new CultureInfo("tr"),
    new CultureInfo("ru"),
    new CultureInfo("es"),
    new CultureInfo("fr")
};
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddLocalization(options => options.ResourcesPath = "");
        builder.Services.AddHttpClient();
        builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services
            .AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.Cookie.Name = "FinalProjectAuth";
        });

        var app = builder.Build();
        localizationOptions.RequestCultureProviders = new IRequestCultureProvider[]
{
    new CookieRequestCultureProvider
    {
        CookieName = CookieRequestCultureProvider.DefaultCookieName
    },
    new AcceptLanguageHeaderRequestCultureProvider()
};


        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRequestLocalization(localizationOptions);
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await DbInitializer.SeedAdminAsync(services);
        }

        app.MapControllerRoute(
            name: "doctorDetails",
            pattern: "doctors/{slug}",
            defaults: new { controller = "Doctors", action = "Details" }
        );

        app.MapControllerRoute(
            name: "admin",
            pattern: "admin/{controller=Appointments}/{action=Index}/{id?}",
            defaults: new { area = "AdminPanel" }
        );

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
        );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        app.Run();
    }
}