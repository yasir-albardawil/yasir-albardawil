using PlateWebAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlateWebAPI.DTOs
{
    public class CategoryUpdatingDTO
    {
        [Required]
        public string CategoryName { get; set; } = String.Empty;
        public string? Description { get; set; }
    }
}
