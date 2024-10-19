using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    [Table("Showtimes")]
    public class Showtime
    {
        public int Id { get; set; }
        public required int TheatreId { get; set; }
        public required int MovieId { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public decimal Price { get; set; }
        public required Movie Movie { get; set; }
        public required Theatre Theatre { get; set; }
    }
}
