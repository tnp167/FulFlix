using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public enum Role
    {
        User,
        Admin,
    }

    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Auth0Id { get; set; } = string.Empty;

        [MaxLength(50)]
        public required string FirstName { get; set; }

        [MaxLength(50)]
        public required string LastName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        public bool EmailVerified { get; set; } = false;

        public string? Picture { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>();

        public string? Location { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Phone { get; set; }

        public bool? PrivacyConsent { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
