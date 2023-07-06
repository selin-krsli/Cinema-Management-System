using Azure;
using CinemaManagementSystem.DATA.Abstract;
using CinemaManagementSystem.ENTITY;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.DATA.Concrete.EfCore
{
    public class EfCoreMovieRepository : EfCoreGenericRepository<Movie, CMSYSTEMContext>, IMovieRepository
    {
        public int GetCountByCategory(string category)
        {
            using (var context = new CMSYSTEMContext())
            {
                var movies = context
                    .Movies
                    .Where(i=>i.IsApproved)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    movies = movies
                        .Include(i => i.MovieCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.MovieCategories.Any(a => a.Category.Url.ToLower() == category.ToLower()));
                }
                return movies.Count();
            }
        }

        public Movie GetMovieDetails(string url)
        {
            using (var context = new CMSYSTEMContext())
            {
                return context.Movies
                              .Where(i => i.Url == url)
                              .Include(i => i.MovieCategories)
                              .ThenInclude(i => i.Category)
                              .FirstOrDefault();
            }
        }

        //public List<Movie> GetPopularMovies()
        //{
        //    using(var context = new CMSYSTEMContext())
        //    {
        //        return context.Movies.ToList();
        //    }
        //}

        public List<Movie> GetRecentMovies()
        {
            using (var context = new CMSYSTEMContext())
            {
                return context.Movies.ToList();
            }
        }

        public List<Movie> GetMoviesByCategory(string name, int page, int pageSize)
        {
            using (var context = new CMSYSTEMContext())
            {
                var movies = context.Movies
                   // .Where(i=>i.IsApproved)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    movies = movies
                        .Include(i => i.MovieCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.MovieCategories.Any(a => a.Category.Url == name.ToLower()));
                }
                return movies.Skip((page - 0) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<Movie> GetSearchResult(string searchingWord)
        {
            using (var context = new CMSYSTEMContext())
            {
                var movies = context.Movies.AsQueryable();
                if (!string.IsNullOrEmpty(searchingWord))
                {
                    movies = movies
                        .Where(i => i.MovieName.ToLower().Contains(searchingWord.ToLower()) || 
                        i.Director.ToLower().Contains(searchingWord.ToLower())||
                        i.Genre.ToLower().Contains(searchingWord.ToLower()));                      
                }
                return movies.ToList();
            }
        }

        public List<Movie> GetHomePageMovies()
        {
            using(var context = new CMSYSTEMContext())
            {
                return context.Movies
                    .Where(i=>i.IsApproved && i.IsHome).ToList();
            }
        }

        public Movie GetByIdWithCategories(int id)
        {
            using(var context = new CMSYSTEMContext())
            {
                return context.Movies
                        .Where(i => i.MovieId == id)
                        .Include(i => i.MovieCategories)
                        .ThenInclude(i => i.Category)
                        .FirstOrDefault();                
            }
        }

        public void Update(Movie entity, int[] categoryIds)
        {
            using(var context = new CMSYSTEMContext())
            {
                var movie = context.Movies
                                    .Include(i=>i.MovieCategories)
                                    .FirstOrDefault(i=>i.MovieId==entity.MovieId);
                if (movie != null)
                {
                    movie.MovieName = entity.MovieName;
                    movie.MovieStory = entity.MovieStory;
                    movie.Genre = entity.Genre;
                    movie.Director = entity.Director;
                    movie.ImageUrl = entity.ImageUrl;
                    movie.Url = entity.Url;
                    movie.MovieCategories = categoryIds.Select(CID => new MovieCategory()
                    { 
                        MovieId = entity.MovieId,
                        CategoryId = CID
                    }).ToList();
                    context.SaveChanges();
                }
            }

        }
    }
}
