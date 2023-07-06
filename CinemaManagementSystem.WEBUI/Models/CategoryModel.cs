using CinemaManagementSystem.ENTITY;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagementSystem.WEBUI.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Display(Prompt = "Enter CategoryName")]
        [Required(ErrorMessage = "CategoryName is a mandatory field to fill in.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Character length must be between 5 and 50!")]
        public string? Name { get; set; }

        [Display(Prompt = "Enter Url")]
        public string? Url { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
