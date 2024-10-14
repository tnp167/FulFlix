using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;

namespace backend.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class PasswordMutation
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetService _passwordResetService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

        public PasswordMutation(
            IUserRepository userRepository,
            IPasswordResetService passwordResetService,
            IPasswordResetTokenRepository passwordResetTokenRepository
        )
        {
            _userRepository = userRepository;
            _passwordResetService = passwordResetService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
        }

        [GraphQLName("sendResetPassword")]
        public async Task<bool> SendResetPasswordAsync(string email)
        {
            var domain = Environment.GetEnvironmentVariable("DOMAIN_URL");
            var token = Guid.NewGuid().ToString();
            var resetLink = $"https://{domain}/reset-password?token={token}";

            var resetToken = new PasswordResetToken
            {
                Token = token,
                Email = email,
                Expires = DateTime.UtcNow.AddHours(24),
            };

            await _passwordResetTokenRepository.CreatePasswordResetTokenAsync(resetToken);

            await _passwordResetService.SendPasswordResetEmail(email, resetLink);

            return true;
        }

        [GraphQLName("passwordReset")]
        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var resetPasswordToken = await _passwordResetTokenRepository.GetPasswordResetTokenAsync(
                token
            );

            if (resetPasswordToken == null || resetPasswordToken.Expires < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired token");
            }

            var user = await _userRepository.GetUserByEmailAsync(resetPasswordToken.Email);
            if (user != null)
            {
                await _passwordResetService.UpdateUserPassword(newPassword, user.Auth0Id);
                await _passwordResetTokenRepository.DeletePasswordResetTokenAsync(
                    resetPasswordToken.Id
                );
            }
            return true;
        }
    }
}
