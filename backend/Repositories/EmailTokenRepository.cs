using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Repositories
{
    public class EmailTokenRepository : IEmailTokenRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public EmailTokenRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateEmailTokenAsync(EmailToken token)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.EmailTokens.AddAsync(token);
            await context.SaveChangesAsync();
        }

        public async Task<EmailToken?> GetEmailTokenAsync(string token)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.EmailTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task DeleteEmailTokenAsync(string id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var token = await context.EmailTokens.FindAsync(id);
            if (token != null)
            {
                context.EmailTokens.Remove(token);
                await context.SaveChangesAsync();
            }
        }
    }
}
