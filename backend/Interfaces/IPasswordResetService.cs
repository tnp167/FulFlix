using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface IPasswordResetService
    {
         Task<bool> UpdateUserPassword(string newPassword, string auth0Id);
         Task SendPasswordResetEmail(string email, string resetLink);
    }
}