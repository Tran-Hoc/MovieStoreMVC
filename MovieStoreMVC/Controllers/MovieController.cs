using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStoreMVC.Models.Domain;
using MovieStoreMVC.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieStoreMVC.Models.DTO;
using MovieStoreMVC.Repositories.Implementation;

namespace MovieStoreMVC.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IGenresService genresService;
        private readonly IMovieService movieService;
        private readonly IFIleService fileSevice;

        public MovieController(IGenresService genresService, IMovieService movieService, IFIleService fIleService)
        {
            this.genresService = genresService;
            this.movieService = movieService;
            this.fileSevice = fIleService;
        }
        public IActionResult Index()
        {
            var data = movieService.List();
            return View(data);
        }
        public IActionResult Add()
        {
            var model = new Movie();
            model.GenreList = genresService.List().Select(
                m => new SelectListItem
                { Text = m.GenreName, Value = m.Id.ToString() });
            return View(model);
        }
        [HttpPost]
        public IActionResult Add(Movie model)
        {
            model.GenreList = genresService.List().Select(
                m => new SelectListItem { Text = m.GenreName, Value = m.Id.ToString() });
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.ImageFile != null)
            {
                var fileResult = fileSevice.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileResult.Item2;
                model.MovieImage = imageName;
            }
            var result = movieService.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction("Add");
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var model = movieService.GetById(id);
            var selectedGenres = movieService.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(genresService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Movie model)
        {
            var selectedGenres = movieService.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(genresService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = fileSevice.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MovieImage = imageName;
            }
            var result = movieService.Update(model);
            if (result)
            {
                TempData["msg"] = "Update Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }
        public IActionResult Delete(int id)
        {
            var result = movieService.Delete(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
