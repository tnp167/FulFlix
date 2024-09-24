using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
        }

       public DbSet<User> Users {get; set;}
       public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
       public DbSet<EmailToken> EmailTokens { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

       }
    }
}