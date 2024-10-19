using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    [Table("Theatres")]
    public class Theatre
    {
        public int Id { get; set; }
        public required int CinemaId { get; set; }
        public required int ScreenNumber { get; set; }
        public required int Rows { get; set; }
        public required int Columns { get; set; }
        public required Cinema Cinema { get; set; }
        public ICollection<Showtime>? Showtimes { get; set; }
        public ICollection<Seat>? Seats { get; set; }
    }
}
