using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override void Dispose() { }

        public override ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
