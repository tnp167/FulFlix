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
    public class EmailMutation
    {
        private readonly IEmailService _emailService;
        private readonly IEmailTokenRepository _emailTokenRepository;
        private readonly IUserRepository _userRepository;

        public EmailMutation(
            IEmailService emailService,
            IEmailTokenRepository emailTokenRepository,
            IUserRepository userRepository
        )
        {
            _emailService = emailService;
            _emailTokenRepository = emailTokenRepository;
            _userRepository = userRepository;
        }

        [GraphQLName("sendVerificationEmail")]
        public async Task<bool> SendVerificationEmailAsync(string email)
        {
            var domain = Environment.GetEnvironmentVariable("DOMAIN_URL");
            var token = Guid.NewGuid().ToString();
            var verificationLink = $"{domain}/verify-email?token={token}";

            var emailToken = new EmailToken
            {
                Token = token,
                Email = email,
                Expires = DateTime.UtcNow.AddHours(24),
            };

            await _emailTokenRepository.CreateEmailTokenAsync(emailToken);

            await _emailService.SendVerificationEmail(email, verificationLink);

            return true;
        }

        [GraphQLName("verifyEmail")]
        public async Task<bool> VerifyEmailAsync(string token)
        {
            var emailToken = await _emailTokenRepository.GetEmailTokenAsync(token);

            if (emailToken == null || emailToken.Expires < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired token");
            }

            var user = await _userRepository.GetUserByEmailAsync(emailToken.Email);
            if (user != null)
            {
                user.EmailVerified = true;
                await _userRepository.UpdateUserAsync(user);
                await _emailTokenRepository.DeleteEmailTokenAsync(emailToken.Id);
            }
            return true;
        }
    }
}
