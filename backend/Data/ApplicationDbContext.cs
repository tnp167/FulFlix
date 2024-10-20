using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext, IDisposable
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }
        public DbSet<EmailToken> EmailTokens { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity
                    .Property(e => e.Roles)
                    .HasConversion(
                        v => string.Join(',', v),
                        v =>
                            v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(r => Enum.Parse<Role>(r))
                                .ToList()
                    );
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasOne(b => b.User).WithMany(u => u.Bookings).HasForeignKey(b => b.UserId);
                entity.HasOne(b => b.Showtime).WithMany().HasForeignKey(b => b.ShowtimeId);
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity
                    .HasOne(s => s.Theatre)
                    .WithMany(t => t.Seats)
                    .HasForeignKey(s => s.TheatreId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BookingSeat>().HasKey(bs => new { bs.BookingId, bs.SeatId });

            modelBuilder
                .Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<BookingSeat>()
                .HasOne(bs => bs.Seat)
                .WithMany(s => s.BookingSeats)
                .HasForeignKey(bs => bs.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Theatre>(entity =>
            {
                entity
                    .HasOne(t => t.Cinema)
                    .WithMany(c => c.Theatres)
                    .HasForeignKey(t => t.CinemaId);
            });

            modelBuilder.Entity<Showtime>(entity =>
            {
                entity
                    .HasOne(s => s.Movie)
                    .WithMany(m => m.Showtimes)
                    .HasForeignKey(s => s.MovieId);
                entity
                    .HasOne(s => s.Theatre)
                    .WithMany(t => t.Showtimes)
                    .HasForeignKey(s => s.TheatreId);
            });
        }

        public async Task SeedDataAsync(ITmdbService tmdbService)
        {
            if (!Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Auth0Id = "auth0|1234567890",
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@example.com",
                        EmailVerified = true,
                        Roles = new List<Role> { Role.Admin, Role.User },
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    },
                    new User
                    {
                        Auth0Id = "auth0|0987654321",
                        FirstName = "Regular",
                        LastName = "User",
                        Email = "user@example.com",
                        EmailVerified = true,
                        Roles = new List<Role> { Role.User },
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    },
                };

                Users.AddRange(users);
                await SaveChangesAsync();
            }

            if (!Cinemas.Any())
            {
                var cinemas = new List<Cinema>
                {
                    new Cinema
                    {
                        Name = "Fulflix Shoreditch",
                        Location = "Shoreditch",
                        Theatres = new List<Theatre>(),
                    },
                    new Cinema
                    {
                        Name = "Fulflix Paddington",
                        Location = "Paddington",
                        Theatres = new List<Theatre>(),
                    },
                    new Cinema
                    {
                        Name = "Fulflix Manchester",
                        Location = "Manchester",
                        Theatres = new List<Theatre>(),
                    },
                    new Cinema
                    {
                        Name = "Fulflix Birmingham",
                        Location = "Birmingham",
                        Theatres = new List<Theatre>(),
                    },
                    new Cinema
                    {
                        Name = "Fulflix Nottingham",
                        Location = "Nottingham",
                        Theatres = new List<Theatre>(),
                    },
                };
                Cinemas.AddRange(cinemas);
                await SaveChangesAsync();

                foreach (var cinema in cinemas)
                {
                    var theatres = new List<Theatre>
                    {
                        new Theatre
                        {
                            CinemaId = cinema.Id,
                            ScreenNumber = 1,
                            Rows = 10,
                            Columns = 15,
                            Cinema = cinema,
                        },
                        new Theatre
                        {
                            CinemaId = cinema.Id,
                            ScreenNumber = 2,
                            Rows = 8,
                            Columns = 12,
                            Cinema = cinema,
                        },
                        new Theatre
                        {
                            CinemaId = cinema.Id,
                            ScreenNumber = 3,
                            Rows = 12,
                            Columns = 15,
                            Cinema = cinema,
                        },
                    };
                    Theatres.AddRange(theatres);
                }
                await SaveChangesAsync();

                foreach (var theatre in Theatres)
                {
                    var seats = new List<Seat>();
                    for (int row = 1; row <= theatre.Rows; row++)
                    {
                        for (int col = 1; col <= theatre.Columns; col++)
                        {
                            seats.Add(
                                new Seat
                                {
                                    TheatreId = theatre.Id,
                                    Row = row,
                                    Column = col,
                                    Theatre = theatre,
                                }
                            );
                        }
                    }
                    Seats.AddRange(seats);
                }
                await SaveChangesAsync();
            }

            if (!Movies.Any())
            {
                var movies = await tmdbService.GetNowPlayingMoviesAsync();
                Movies.AddRange(movies);
                await SaveChangesAsync();

                var random = new Random();
                foreach (var movie in movies)
                {
                    foreach (var theatre in Theatres)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var startTime = DateTime
                                .Now.AddDays(random.Next(1, 30))
                                .AddHours(random.Next(10, 23))
                                .AddMinutes(random.Next(0, 60));

                            var endTime = startTime.AddMinutes(movie.Runtime);
                            Showtimes.Add(
                                new Showtime
                                {
                                    MovieId = movie.Id,
                                    TheatreId = theatre.Id,
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    Price = random.Next(10, 20),
                                    Movie = movie,
                                    Theatre = theatre,
                                }
                            );
                        }
                    }
                }
                await SaveChangesAsync();
            }
        }

        public override void Dispose() { }

        public override ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
