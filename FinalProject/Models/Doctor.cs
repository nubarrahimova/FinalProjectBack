using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class Doctor
{
    public int Id { get; set; }

    [Required, StringLength(140)]
    public string FullName { get; set; } = "";

    [Required, StringLength(140)]
    public string Slug { get; set; } = "";

    [StringLength(200)]
    public string? Clinic { get; set; }

    public int ExperienceYears { get; set; }

    [StringLength(300)]
    public string? PhotoUrl { get; set; }

    public int SpecialityId { get; set; }
    public Speciality? Speciality { get; set; }
}