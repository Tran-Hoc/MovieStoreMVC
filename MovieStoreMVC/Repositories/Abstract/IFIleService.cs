namespace MovieStoreMVC.Repositories.Abstract
{
    public interface IFIleService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
