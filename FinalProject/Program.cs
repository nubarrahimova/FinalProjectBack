using Microsoft.AspNetCore.Mvc;
using FinalProject.Data;
using Microsoft.EntityFrameworkCore;

namespace FinalProject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });
        builder.Services.AddDbContext<AppDbContext>
            (options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

        var app = builder.Build();
        DbInitializer.Seed(app);

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapControllerRoute(
            name: "doctorDetails",
            pattern: "doctors/{slug}",
            defaults: new { controller = "Doctors", action = "Details" }
        );

        app.MapControllerRoute(
            name: "booking",
            pattern: "booking/{slug}",
            defaults: new { controller = "Booking", action = "Create" }
        );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        app.Run();
    }
}