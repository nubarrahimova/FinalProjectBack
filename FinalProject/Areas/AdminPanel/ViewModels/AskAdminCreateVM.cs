using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class AskAdminCreateVM
    {
        [Required(ErrorMessage = "Kim olduğunuzu seçin")]
        public string UserType { get; set; } = "Pasiyent";

        [Required(ErrorMessage = "Ad daxil edin")]
        [MaxLength(100)]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Telefon daxil edin")]
        [MaxLength(30)]
        public string Phone { get; set; } = "";

        [EmailAddress(ErrorMessage = "Email formatı düzgün deyil")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mövzu daxil edin")]
        [MaxLength(200)]
        public string Subject { get; set; } = "";

        [Required(ErrorMessage = "Mesaj daxil edin")]
        [MaxLength(2000)]
        public string Message { get; set; } = "";
    }
}