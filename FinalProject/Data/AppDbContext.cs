using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Speciality> Specialities => Set<Speciality>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>()
            .HasIndex(x => x.Slug)
            .IsUnique();
    }
}