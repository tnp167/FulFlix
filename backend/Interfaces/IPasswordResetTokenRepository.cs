using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
namespace backend.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task CreatePasswordResetTokenAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token);
        Task DeletePasswordResetTokenAsync(string id);
    }
}