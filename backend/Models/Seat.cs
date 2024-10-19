using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    [Table("Seats")]
    public class Seat
    {
        public int Id { get; set; }
        public required int TheatreId { get; set; }
        public required int Row { get; set; }
        public required int Column { get; set; }
        public required Theatre Theatre { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}
