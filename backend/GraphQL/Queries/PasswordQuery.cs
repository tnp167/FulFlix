using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Repositories;
using backend.Interfaces;

namespace backend.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class PasswordQuery
    {  
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IPasswordResetService _passwordResetService;
        public PasswordQuery(IUserRepository userRepository, IPasswordResetTokenRepository passwordResetTokenRepository, IPasswordResetService passwordResetService)
        {
          _userRepository = userRepository;
          _passwordResetTokenRepository = passwordResetTokenRepository;
          _passwordResetService = passwordResetService;
        }

        [GraphQLName("passwordReset")]
        public async Task<bool> ResetPasswordAsync (string token, string newPassword){
            var resetPasswordToken = await _passwordResetTokenRepository.GetPasswordResetTokenAsync(token);

            if(resetPasswordToken == null || resetPasswordToken.Expires < DateTime.UtcNow){
                throw new Exception("Invalid or expired token");
            }

            var user = await _userRepository.GetUserByEmailAsync(resetPasswordToken.Email);
            if(user != null){
                await _passwordResetService.UpdateUserPassword(newPassword, user.Auth0Id);
                await _passwordResetTokenRepository.DeletePasswordResetTokenAsync(resetPasswordToken.Id);
            }
            return true;
        }
    }
}