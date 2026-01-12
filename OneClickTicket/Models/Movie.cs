using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OneClickTicket.Models
{
    public enum GenreType { Action, Comedy, Drama, Fantasy, Horror, Mystery, Romance, Thriller }

    [Table("Movies")]
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Range(30, 240)]
        public int Duration { get; set; } 

        public GenreType Genre { get; set; }

        [Required, StringLength(100)]
        public string Director { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
