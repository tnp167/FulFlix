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
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public PasswordResetTokenRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreatePasswordResetTokenAsync(PasswordResetToken token)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.PasswordResetTokens.AddAsync(token);
            await context.SaveChangesAsync();
        }

        public async Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.PasswordResetTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task DeletePasswordResetTokenAsync(string id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var token = await context.PasswordResetTokens.FindAsync(id);
            if (token != null)
            {
                context.PasswordResetTokens.Remove(token);
                await context.SaveChangesAsync();
            }
        }
    }
}
