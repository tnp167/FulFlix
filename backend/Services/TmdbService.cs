using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Newtonsoft.Json;

namespace backend.Services
{
    public class TmdbService : ITmdbService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly ILogger<TmdbService> _logger;

        public TmdbService(IHttpClientFactory httpClientFactory, ILogger<TmdbService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY") ?? string.Empty;
            _logger = logger;
        }

        public async Task<List<Movie>> GetNowPlayingMoviesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(
                    $"https://api.themoviedb.org/3/movie/now_playing?api_key={_apiKey}"
                );
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TmdbNowPlayingResponseDto>(content);

                if (result?.Results == null || !result.Results.Any())
                {
                    _logger.LogWarning("No movies returned from TMDB API.");
                    return new List<Movie>();
                }

                var movies = new List<Movie>();
                foreach (var m in result.Results)
                {
                    var (runtime, genres) = await GetMovieDetailsAsync(m.Id);
                    movies.Add(
                        new Movie
                        {
                            TmdbId = m.Id,
                            Title = m.Title,
                            Overview = m.Overview,
                            PosterPath = m.PosterPath ?? "",
                            BackdropPath = m.BackdropPath ?? "",
                            ReleaseDate = m.ReleaseDate,
                            Runtime = runtime,
                            Genre = string.Join(", ", genres),
                            Popularity = m.Popularity,
                            VoteAverage = m.VoteAverage,
                        }
                    );
                }

                return movies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching now playing movies");
                return new List<Movie>();
            }
        }

        public async Task<(int Runtime, List<string> Genres)> GetMovieDetailsAsync(int tmdbId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(
                    $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={_apiKey}"
                );
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<MovieDetailsDto>(content);
                    return (
                        result?.Runtime ?? 120,
                        result?.Genres.Select(g => g.Name).ToList() ?? new List<string>()
                    );
                }
                _logger.LogWarning("Failed to fetch movie details for TMDB ID: {TmdbId}", tmdbId);
                return (0, new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching movie details for TMDB ID: {TmdbId}", tmdbId);
                return (0, new List<string>());
            }
        }
    }
}
