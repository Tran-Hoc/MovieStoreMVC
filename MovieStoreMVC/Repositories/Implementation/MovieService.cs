using MovieStoreMVC.Models.Domain;
using MovieStoreMVC.Models.DTO;
using MovieStoreMVC.Repositories.Abstract;

namespace MovieStoreMVC.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DatabaseContext context;

        public MovieService(DatabaseContext context)
        {
            this.context = context;
        }
        public bool Add(Movie model)
        {
            try
            {
                context.Movie.Add(model);
                context.SaveChanges();
                foreach (int genreid in model.Genres)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = genreid,
                        GenreId = genreid,
                    };
                    context.MovieGenre.Add(movieGenre);
                }
                context.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;

                var movieGenre = context.MovieGenre.Where(a => a.MovieId == data.Id);

                foreach (var genre in movieGenre)
                {
                    context.MovieGenre.Remove(genre);
                }
                context.Movie.Remove(data);
                context.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public Movie GetById(int id)
        {
            return context.Movie.Find(id);
        }

        public List<int> GetGenreByMovieId(int movieId)
        {
            var genreID = context.MovieGenre.Where(a => a.MovieId == movieId).Select(a => a.GenreId).ToList();
            return genreID;
        }

        public MovieListVm List(string term = "", bool paging = false, int currentPage = 0)
        {
            var data = new MovieListVm();
            var list = context.Movie.ToList();
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().StartsWith(term)).ToList();
            }
            if (paging)
            {
                int pagesize = 5;
                int count = list.Count;
                int totalPage = (int)Math.Ceiling(count / (double)pagesize);
                list = list.Skip((currentPage - 1) * pagesize).Take(pagesize).ToList();
                data.PageSize = pagesize;
                data.CurrentPage = currentPage;
                data.TotalPages = totalPage;
            }
            foreach (var movie in list)
            {
                var genres = (from genre in context.Genre
                              join mg in context.MovieGenre
                              on genre.Id equals mg.GenreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreName = string.Join(",", genres);
                movie.GenreNames = genreName;
            }
            data.MovieList = list.AsQueryable();
            return data;
        }

        public bool Update(Movie model)
        {
            try
            {
                var genresToDeleted = context.MovieGenre.Where(a => a.MovieId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();
                foreach (var genre in genresToDeleted)
                {
                    context.MovieGenre.Remove(genre);
                }
                foreach (int genid in model.Genres)
                {
                    var movieGenres = context.MovieGenre.FirstOrDefault(a => a.MovieId == model.Id && a.GenreId == genid);
                    if (movieGenres == null)
                    {
                        movieGenres = new MovieGenre { GenreId = genid, MovieId = model.Id };
                        context.MovieGenre.Add(movieGenres);
                    }
                }
                context.Movie.Update(model);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
