using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.DTOs;

namespace backend.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByAuth0IdAsync(string auth0Id);
        Task UpdateUserAsync(User user);
    }
}