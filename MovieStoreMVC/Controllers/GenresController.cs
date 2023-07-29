using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStoreMVC.Models.Domain;
using MovieStoreMVC.Repositories.Abstract;
using MovieStoreMVC.Repositories.Implementation;

namespace MovieStoreMVC.Controllers
{
    [Authorize]
    public class GenresController : Controller
    {
        private readonly IGenresService genreSevice;

        public GenresController(IGenresService genresService)
        {
            this.genreSevice = genresService;
        }
        public IActionResult Index()
        {
            var data = this.genreSevice.List().ToList();
            return View(data);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Genres model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = this.genreSevice.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Seccessfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }
        public IActionResult Edit(int id)
        {
            var data = this.genreSevice.GetById(id);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Genres model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = this.genreSevice.Update(model);
            if (result)
            {
                TempData["msg"] = "Update Seccessfully";
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
            var result = this.genreSevice.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
