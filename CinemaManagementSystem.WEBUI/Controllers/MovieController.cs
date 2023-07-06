using CinemaManagementSystem.BUSINESS.Abstract;
using CinemaManagementSystem.DATA;
using CinemaManagementSystem.DATA.Concrete.EfCore;
using CinemaManagementSystem.ENTITY;
using CinemaManagementSystem.WEBUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace CinemaManagementSystem.WEBUI.Controllers
{
    public class MovieController:Controller
    {

        private IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;           
        }


        public IActionResult List(string category, int page)
        {
            const int pageSize = 3;
            var movieViewModel = new MovieListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _movieService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Movies = _movieService.GetMoviesByCategory(category, page, pageSize)
            };

            return View(movieViewModel);
        }
        [HttpGet]
        public IActionResult Details(string url)
        {
            if(url == null)
            {
                return NotFound();
            }
            Movie movie = _movieService.GetMovieDetails(url);
            if(movie==null)
            {
                return NotFound();
            }
            return View(new MovieDetailModel
            {
                Movie = movie,
                Categories = movie.MovieCategories.Select(i => i.Category).ToList()
            });
        }
        public IActionResult Search(string q)
        {
            var movieViewModel = new MovieListViewModel()
            {
                Movies = _movieService.GetSearchResult(q)
            };
            return View(movieViewModel);
        }

        public IActionResult MovieInfo()
        {
            
            
            return View();
        }
    }
}

