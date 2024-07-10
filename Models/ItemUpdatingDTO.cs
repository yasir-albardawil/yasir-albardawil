using PlateWebAPI.DTOs;
using PlateWebAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace PlateWebAPI.Models
{
    public class ItemUpdatingDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        [Required]
        public string? LongDescription { get; set; }

        public string? AllergyInformation { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public string? ImageThumbnailUrl { get; set; }

        public bool IsPieOfTheWeek { get; set; }

        public bool InStock { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
