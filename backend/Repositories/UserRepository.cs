using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using backend.DTOs;
using AutoMapper;

namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
              _context = context;
              _mapper = mapper;
        }
        public async Task<User> CreateUserAsync(User user)
        { 
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByAuth0IdAsync(string auth0Id)
        {   
            return await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);
        }

        public async Task UpdateUserAsync(User user)
        {
      
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}