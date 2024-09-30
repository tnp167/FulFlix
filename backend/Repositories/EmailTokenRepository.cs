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
    public class EmailTokenRepository : IEmailTokenRepository
    {
        private readonly ApplicationDbContext _context;
        public EmailTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateEmailTokenAsync(EmailToken token)
        {
            await _context.EmailTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<EmailToken?> GetEmailTokenAsync(string token)
        {
            return await _context.EmailTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task DeleteEmailTokenAsync(string id)
        {
            var token = await _context.EmailTokens.FindAsync(id);
            if (token != null)
            {
                _context.EmailTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }


    }
}