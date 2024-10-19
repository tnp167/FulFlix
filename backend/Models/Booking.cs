using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public enum Status
    {
        Reserved,
        Confirmed,
        Cancelled,
        Completed,
        Pending,
    }

    [Table("Bookings")]
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public DateTime BookingTime { get; set; }
        public required string Status { get; set; }
        public decimal TotalPrice { get; set; }

        public required User User { get; set; }
        public required Showtime Showtime { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}
