using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public enum Role {
        User,
        Admin 
    }

    [Table("Users")]
    public class User
    {
        [Key]
        public int Id {get; set;}

        public string Auth0Id { get; set; } = string.Empty;

        [MaxLength(50)]
        public required string FirstName { get; set; }

        [MaxLength(50)]
        public required string LastName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        public bool EmailVerified { get; set; } = false;

        public string? Image { get; set; }

        [MaxLength(256)]
        public required string Password { get; set; } 

        public List<Role> Roles { get; set; } = new List<Role>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

    }
}