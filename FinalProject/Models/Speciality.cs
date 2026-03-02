using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class Speciality
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = "";

    public List<Doctor> Doctors { get; set; } = new();
}