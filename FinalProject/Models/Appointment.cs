using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
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

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

       
        [Required, StringLength(20)]
        public string Time { get; set; } = "";

        [StringLength(500)]
        public string? Comment { get; set; }

        // Doctor FK
        [Required]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        // Admin panel üçün status
        [Required, StringLength(30)]
        public string Status { get; set; } = "New";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}