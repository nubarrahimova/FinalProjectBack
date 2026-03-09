using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FinalProject.ViewModels.AdminPanel
{
    public class ArticleFormVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Başlıq boş ola bilməz")]
        [StringLength(200)]
        public string Title { get; set; } = "";

        [StringLength(500)]
        public string? Summary { get; set; }

        [Required(ErrorMessage = "Mezmun boş ola bilməz")]
        public string Content { get; set; } = "";

        public IFormFile? CoverImageFile { get; set; }

        public string? ExistingImageUrl { get; set; }

        public bool IsPublished { get; set; }

        public string? AuthorName { get; set; }
        public string? AuthorSpecialty { get; set; }
    }
}