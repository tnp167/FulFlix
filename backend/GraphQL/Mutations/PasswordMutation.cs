using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using backend.Models;
using backend.Interfaces;

namespace backend.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class PasswordMutation
    {
        private readonly IPasswordResetService _passwordResetService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        
        public PasswordMutation(IPasswordResetService passwordResetService, IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _passwordResetService = passwordResetService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
        }

        [GraphQLName("sendResetPassword")]
        public async Task<bool> SendResetPasswordAsync(string email){
            var domain = Environment.GetEnvironmentVariable("DOMAIN_URL");
            var token = Guid.NewGuid().ToString();
            var resetLink = $"https://{domain}/reset-password?token={token}";

            var resetToken = new PasswordResetToken
            {
                Token = token,
                Email = email,
                Expires = DateTime.UtcNow.AddHours(24) 
            };

            await _passwordResetTokenRepository.CreatePasswordResetTokenAsync(resetToken);

            await _passwordResetService.SendPasswordResetEmail(email, resetLink);

            return true;
        }
    }
}