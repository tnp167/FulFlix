using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs;

namespace backend.Interfaces
{
    public interface IAuth0Client
    {
         Task<Auth0UserResponseDto?> CreateAuth0UserAsync(Auth0UserRequestDto request);
         Task<bool> AssignRoleToUserAsync(string userId, string roleName);
         Task<string?> LoginAuth0UserAsync (string email, string password);
         Task<string> GetManagementApiTokenAsync();
    }
}