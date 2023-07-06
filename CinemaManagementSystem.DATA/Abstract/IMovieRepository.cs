using CinemaManagementSystem.ENTITY;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.DATA.Abstract
{
    public interface IMovieRepository:IRepository<Movie>
    {
        Movie GetMovieDetails(string url);
        List<Movie> GetMoviesByCategory(string name, int page, int pageSize);
        Movie GetByIdWithCategories(int id);
        List<Movie> GetSearchResult(string searchingWord);
        List<Movie> GetRecentMovies();
        List<Movie> GetHomePageMovies();
        int GetCountByCategory(string category);
        void Update(Movie entity, int[] categoryIds);
    }
}
