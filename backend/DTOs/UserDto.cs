using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Auth0Id { get; set; } = string.Empty;

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public bool EmailVerified { get; set; }

        public string? Image { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CreateUserDto {
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [MaxLength(50)]
        public required string LastName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [MaxLength(256)]
        public required string Password { get; set; } 
    }

   public class Auth0UserRequestDto
    {
        [JsonProperty("email")]
        public required string Email { get; set; }

        [JsonProperty("password")]
        public required string Password { get; set; }

        [JsonProperty("connection")]
        public string Connection { get; set; } = "Username-Password-Authentication"; 

        [JsonProperty("user_metadata")]
        public required Auth0UserMetadataDto UserMetadata { get; set; }
    }

    public class Auth0UserResponseDto
    {
        [JsonProperty("user_id")]
        public required string UserId { get; set; }

        [JsonProperty("email")]
        public required string Email { get; set; }

        [JsonProperty("user_metadata")]
        public required Auth0UserMetadataDto UserMetadata { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

    }

    public class Auth0UserMetadataDto
    {
        [JsonProperty("first_name")]
        public required string FirstName { get; set; }

        [JsonProperty("last_name")]
        public required string LastName { get; set; }
    }

    public class Auth0RoleDto
    {
        [JsonProperty("id")]
        public required string Id { get; set; }

        [JsonProperty("name")]
        public required string Name { get; set; }
    }

    public class LoginUserDto{
        public required string Email { get; set;}
        public required string Password { get; set;}
    }

    public class Auth0TokenResponse{
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("id_token")]
        public string? IdToken { get; set; }

        [JsonProperty("token_type")]
        public string? TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

}