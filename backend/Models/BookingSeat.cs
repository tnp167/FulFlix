using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    [Table("BookingSeats")]
    public class BookingSeat
    {
        public int Id { get; set; }
        public required int BookingId { get; set; }
        public required int SeatId { get; set; }
        public required Booking Booking { get; set; }
        public required Seat Seat { get; set; }
    }
}
