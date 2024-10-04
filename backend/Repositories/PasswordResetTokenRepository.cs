using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using backend.DTOs;

namespace backend.Repositories
{
    public class PasswordResetTokenRepository: IPasswordResetTokenRepository
    {
         private readonly ApplicationDbContext _context;
         public PasswordResetTokenRepository(ApplicationDbContext context)
         {
             _context = context;
         }
        public async Task CreatePasswordResetTokenAsync(PasswordResetToken token)
         {
            await _context.PasswordResetTokens.AddAsync(token);
            await _context.SaveChangesAsync();
         }

        public async Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token)
        {
            return await _context.PasswordResetTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task DeletePasswordResetTokenAsync(string id)
        {
            var token = await _context.PasswordResetTokens.FindAsync(id);
            if (token != null)
            {
                _context.PasswordResetTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}