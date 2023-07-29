using Humanizer.Localisation.DateToOrdinalWords;
using Microsoft.AspNetCore.Mvc;
using MovieStoreMVC.Repositories.Abstract;

namespace MovieStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService mservice;

        public HomeController(IMovieService mservice)
        {
            this.mservice = mservice;
        }
        public IActionResult Index(string term ="", int currePage = 1)
        {
            var movies = this.mservice.List(term, true, currePage);
            return View(movies);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult MovieDetail(int movieId)
        {
            var movie = this.mservice.GetById(movieId);
            return View(movie);
        }

    }
}
