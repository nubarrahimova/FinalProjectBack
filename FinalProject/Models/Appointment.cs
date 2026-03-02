using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class Appointment
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string FirstName { get; set; } = "";

    [Required, StringLength(60)]
    public string LastName { get; set; } = "";

    [Required, StringLength(30)]
    public string Phone { get; set; } = "";

    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required, StringLength(10)]
    public string Time { get; set; } = "";

    [StringLength(500)]
    public string? Comment { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    [StringLength(30)]
    public string Status { get; set; } = "New";
}
