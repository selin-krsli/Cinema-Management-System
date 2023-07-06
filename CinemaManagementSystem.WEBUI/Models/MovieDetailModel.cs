using CinemaManagementSystem.ENTITY;

namespace CinemaManagementSystem.WEBUI.Models
{
    public class MovieDetailModel
    {
        public Movie Movie { get; set; }
        public List<Category> Categories { get; set; }
    }
}
