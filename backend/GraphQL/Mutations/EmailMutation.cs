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
    public class EmailMutation
    {
        private readonly IEmailService _emailService;
        private readonly IEmailTokenRepository _emailTokenRepository;

        public EmailMutation(IEmailService emailService, IEmailTokenRepository emailTokenRepository){
            _emailService = emailService;
            _emailTokenRepository = emailTokenRepository;
        }
        

        [GraphQLName("sendVerificationEmail")]
        public async Task<bool> SendVerificationEmailAsync(string email){
            var domain = Environment.GetEnvironmentVariable("DOMAIN_URL");
            var token = Guid.NewGuid().ToString();
            var verificationLink = $"https://{domain}/verify-email?token={token}";

            var emailToken = new EmailToken
            {
                Token = token,
                Email = email,
                Expires = DateTime.UtcNow.AddHours(24) 
            };

            await _emailTokenRepository.CreateEmailTokenAsync(emailToken);

            await _emailService.SendVerificationEmail(email, verificationLink);

            return true;
        }
    }
}