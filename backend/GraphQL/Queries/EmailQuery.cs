using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Repositories;
using backend.Interfaces;

namespace backend.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class EmailQuery
    {
        private readonly IEmailTokenRepository _emailTokenRepository;
        private readonly IUserRepository _userRepository;

        public EmailQuery(IEmailTokenRepository emailTokenRepository, IUserRepository userRepository)
        {
            _emailTokenRepository = emailTokenRepository;
            _userRepository = userRepository;
        }

        [GraphQLName("emailVerification")]
        public async Task<bool> VerifyEmailAsync (string token){
            var emailToken = await _emailTokenRepository.GetEmailTokenAsync(token);

            if(emailToken == null || emailToken.Expires < DateTime.UtcNow){
                throw new Exception("Invalid or expired token");
            }

            var user = await _userRepository.GetUserByEmailAsync(emailToken.Email);
            if(user != null){
                user.EmailVerified = true;
                await _userRepository.UpdateUserAsync(user);
                await _emailTokenRepository.DeleteEmailTokenAsync(emailToken.Id);
            }
            return true;
        }
    }
}