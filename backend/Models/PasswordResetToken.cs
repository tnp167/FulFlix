using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("PasswordResetToken")]
    public class PasswordResetToken
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Token { get; set; } 

        public DateTime Expires { get; set; } 

        [EmailAddress]
        public required string Email { get; set; } 
}
}