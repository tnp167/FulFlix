using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backend.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;

        [JsonProperty("poster_path")]
        public string? PosterPath { get; set; }

        [JsonProperty("backdrop_path")]
        public string? BackdropPath { get; set; }

        [JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }

        public double Popularity { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        public List<int> GenreIds { get; set; } = new List<int>();
    }

    public class TmdbNowPlayingResponseDto
    {
        public required List<MovieDto> Results { get; set; }
    }

    public class MovieDetailsDto
    {
        public int Runtime { get; set; }
        public required List<GenreDto> Genres { get; set; }
    }

    public class GenreDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
