using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email boş ola bilməz.")]
        [EmailAddress(ErrorMessage = "Düzgün emaili daxil edin.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifrə boş ola bilməz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}