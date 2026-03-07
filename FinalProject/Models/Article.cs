using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = "";

        [Required, StringLength(220)]
        public string Slug { get; set; } = "";

        [StringLength(500)]
        public string? Summary { get; set; }

        [Required]
        public string Content { get; set; } = "";

        [StringLength(500)]
        public string? CoverImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsPublished { get; set; }
    }
}