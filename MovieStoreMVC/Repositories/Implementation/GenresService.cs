using MovieStoreMVC.Models.Domain;
using MovieStoreMVC.Repositories.Abstract;
using System.Text.RegularExpressions;

namespace MovieStoreMVC.Repositories.Implementation
{
    public class GenresService : IGenresService
    {
        private readonly DatabaseContext context;

        public GenresService(DatabaseContext context)
        {
            this.context = context;

        }
        public bool Add(Genres model)
        {
            try
            {
                context.Genre.Add(model);
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
                context.Genre.Remove(data);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Genres GetById(int id)
        {
            return context.Genre.Find(id);
        }

        public IQueryable<Genres> List()
        {
            return context.Genre.AsQueryable();
        }

        public bool Update(Genres model)
        {
            try
            {
                context.Genre.Update(model);
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
