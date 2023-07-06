using CinemaManagementSystem.BUSINESS.Abstract;
using CinemaManagementSystem.DATA.Abstract;
using CinemaManagementSystem.DATA.Concrete.EfCore;
using CinemaManagementSystem.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.BUSINESS.Concrete
{
    public class MovieManager : IMovieService
    {
        private IMovieRepository _movieRepository;
        public MovieManager(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public void Create(Movie entity)
        {
            _movieRepository.Create(entity);
        }

        public void Delete(Movie entity)
        {
            _movieRepository.Delete(entity);
        }

        public List<Movie> GetAll()
        {
            return _movieRepository.GetAll();
        }

        public Movie GetById(int id)
        {
            return _movieRepository.GetById(id);
        }

        public Movie GetByIdWithCategories(int id)
        {
            return _movieRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
           return _movieRepository.GetCountByCategory(category);
        }

        public List<Movie> GetHomePageMovies()
        {
            return _movieRepository.GetHomePageMovies();
        }

        public Movie GetMovieDetails(string url)
        {
            return _movieRepository.GetMovieDetails(url);
        }

        public List<Movie> GetMoviesByCategory(string name, int page, int pageSize)
        {
            return _movieRepository.GetMoviesByCategory(name, page, pageSize);
        }

        public List<Movie> GetSearchResult(string searchingWord)
        {
           return _movieRepository.GetSearchResult(searchingWord);
        }

        public void Update(Movie entity)
        {
            _movieRepository.Update(entity);    
        }

        public void Update(Movie entity, int[] categoryIds)
        {
            _movieRepository.Update(entity, categoryIds);
        }
    }
}
