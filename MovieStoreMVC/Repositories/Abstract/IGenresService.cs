using MovieStoreMVC.Models.Domain;

namespace MovieStoreMVC.Repositories.Abstract
{
    public interface IGenresService
    {
        bool Add(Genres model);
        bool Update(Genres model);
        Genres GetById(int id);
        bool Delete(int id);
        IQueryable<Genres> List();
    }
}
