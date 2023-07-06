using CinemaManagementSystem.ENTITY;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagementSystem.WEBUI.Models
{
    public class MovieModel
    {
        public int MovieId { get; set; }

        [Display(Prompt ="Enter MovieName")]
        //[Required(ErrorMessage = "MovieName is a mandatory field to fill in.")]
        //[StringLength(50, MinimumLength = 5, ErrorMessage = "Character length must be between 5 and 50!")]
        public string MovieName { get; set; }
        [Display(Prompt = "Enter Url")]
        [Required(ErrorMessage = "Url is a mandatory field to fill in.")]
        public string Url { get; set; }

        [Display(Prompt = "Enter MovieStory")]
        //[Required(ErrorMessage = "MovieStory is a mandatory field to fill in.")]
        //[StringLength(2000, MinimumLength =20, ErrorMessage = "Character length must be between 20 and 2000!")]
        public string MovieStory { get; set; }
        public int ReleaseYear { get; set; }
        //public int Duration { get; set; }

        [Display(Prompt = "Enter Genre")]
       //[Required(ErrorMessage = "Genre is a mandatory field to fill in.")]
        public string Genre { get; set; }

        [Display(Prompt = "Enter Director")]
        //[Required(ErrorMessage = "Director is a mandatory field to fill in.")]
        public string Director { get; set; }
        public decimal Price { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }

        [Display(Prompt = "Enter ImageUrl")]
        //[Required(ErrorMessage = "ImageUrl is a mandatory field to fill in.")]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }    
        public int CategoryId { get; set; }
        public List<MovieCategory> MovieCategories { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}
