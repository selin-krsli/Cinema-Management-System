using CinemaManagementSystem.BUSINESS.Abstract;
using CinemaManagementSystem.DATA.Abstract;
using CinemaManagementSystem.WEBUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CinemaManagementSystem.WEBUI.Controllers
{
    public class HomeController : Controller
    {
        private IMovieService _movieService;
        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        public IActionResult Index()
        {
            var movieViewModel = new MovieListViewModel()
            {
                Movies = _movieService.GetAll(),
            };
            return View(movieViewModel);
        }
         public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View("MyView");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}