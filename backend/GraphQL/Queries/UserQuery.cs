using System.Security.Claims;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Interfaces;
using HotChocolate;
using HotChocolate.Authorization;

namespace backend.GraphQL
{
    [ExtendObjectType("Query")]
    public class UserQuery
    {
        private readonly IUserService _userService;

        public UserQuery(IUserService userService)
        {
            _userService = userService;
        }

        [GraphQLName("user")]
        [Authorize]
        public async Task<UserDto?> GetUserProfile(ClaimsPrincipal claimsPrincipal)
        {
            var auth0Id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (auth0Id == null)
            {
                throw new Exception("Invalid token.");
            }

            return await _userService.GetUserProfileAsync(auth0Id);
        }
    }
}
