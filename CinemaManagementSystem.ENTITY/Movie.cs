using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.ENTITY
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string? MovieName { get; set; }
        public string? Url { get; set; }
        public string? MovieStory { get; set; }
        public int ReleaseYear { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public string? Language { get; set; }
       //public string? Country { get; set; }
        public string? ImageUrl { get; set; }
       public string? TrailerUrl { get; set; }
        
        //[Precision(18, 2)]
        public decimal Price { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public int CategoryId { get; set; }
        public List<MovieCategory>? MovieCategories { get; set; } 
    }
}
