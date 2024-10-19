using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    [Table("Movies")]
    public class Movie
    {
        public int Id { get; set; }
        public required int TmdbId { get; set; }
        public required string Title { get; set; }
        public required string Overview { get; set; }
        public required string PosterPath { get; set; }
        public required int Runtime { get; set; }
        public required double Popularity { get; set; }
        public required string BackdropPath { get; set; }
        public required string Genre { get; set; }
        public required DateTime ReleaseDate { get; set; }
        public double VoteAverage { get; set; }

        public ICollection<Showtime>? Showtimes { get; set; }
    }
}
