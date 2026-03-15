using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class AdminQuestion
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserType { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = "";

        [Required]
        [MaxLength(30)]
        public string Phone { get; set; } = "";

        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = "";

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}