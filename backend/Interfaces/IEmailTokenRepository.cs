using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
namespace backend.Interfaces
{
    public interface IEmailTokenRepository
    {
        Task CreateEmailTokenAsync(EmailToken token);
        Task<EmailToken?> GetEmailTokenAsync(string token);
        Task DeleteEmailTokenAsync(string id);
    }
}