using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OneClickTicket.Models
{
    [Table("Cinemas")]
    public class Cinema
    {
        [Key]
        public int CinemaId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(200)]
        public string Location { get; set; }

        [Range(1, 20)]
        public int NumberOfHalls { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
