using System.ComponentModel.DataAnnotations;

namespace MovieStoreMVC.Models.Domain
{
    public class Genres
    {
        public int Id { get; set; }
        [Required]
        public string? GenreName { get; set; }
    }
}
