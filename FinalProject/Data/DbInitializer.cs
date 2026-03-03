using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data;

public static class DbInitializer
{
    public static void Seed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Migrations varsa tətbiq edir (DB hazır olsun deyə)
        db.Database.Migrate();

        // DB-də artıq doctor varsa, bir də doldurma
        if (db.Doctors.Any() || db.Specialities.Any())
            return;

        // 1) Speciality-lər
        var ginekolog = new Speciality { Name = "Ginekoloq" };
        var pediatr = new Speciality { Name = "Pediatr" };
        var kardioloq = new Speciality { Name = "Kardioloq" };

        db.Specialities.AddRange(ginekolog, pediatr, kardioloq);
        db.SaveChanges();

        // 2) Doctors
        db.Doctors.AddRange(
            new Doctor
            {
                FullName = "Dr. Xuraman Qaribova",
                Slug = "dr-xuraman-qaribova",
                ExperienceYears = 17,
                Clinic = "HTCcliniva hospital",
                PhotoUrl = "/assets/images/xuraman-qaribova.jpg",
                SpecialityId = ginekolog.Id
            },
            new Doctor
            {
                FullName = "Dr. Ceyran İmaməliyeva",
                Slug = "dr-ceyran-imameliyeva",
                ExperienceYears = 9,
                Clinic = "4 Saylı Qadın Məsləhətxanası",
                PhotoUrl = "/assets/images/ceyran-imamaliyeva.jpg",
                SpecialityId = ginekolog.Id
            },
            new Doctor
            {
                FullName = "Dr. Xənım Bakırova",
                Slug = "dr-xanim-bakirova",
                ExperienceYears = 13,
                Clinic = "HTCcliniva hospital",
                PhotoUrl = "/assets/images/xanim-bakirova.jpg",
                SpecialityId = pediatr.Id
            }
        );

        db.SaveChanges();
    }
}