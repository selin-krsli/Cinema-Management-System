using CinemaManagementSystem.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.BUSINESS.Abstract
{
    public interface IMovieService
    {
        Movie GetMovieDetails(string url);
        Movie GetById(int id);
        Movie GetByIdWithCategories(int id);
        List<Movie> GetMoviesByCategory(string name,int page, int pageSize);
        List<Movie> GetAll();
        List<Movie> GetSearchResult(string searchingWord);
        List<Movie> GetHomePageMovies();
        void Create(Movie entity);
        void Update(Movie entity);
        void Delete(Movie entity);
        int GetCountByCategory(string category);
        void Update(Movie entity, int[] categoryIds);
    }
}
