using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class BookingCreateVM
    {
        public string DoctorSlug { get; set; } = "";
        public string DoctorName { get; set; } = "";

        [Required, StringLength(50)]
        public string FirstName { get; set; } = "";

        [Required, StringLength(60)]
        public string LastName { get; set; } = "";

        [Required, StringLength(30)]
        public string Phone { get; set; } = "";

        [EmailAddress]
        public string? Email { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required]
        public string Time { get; set; } = "";

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}
