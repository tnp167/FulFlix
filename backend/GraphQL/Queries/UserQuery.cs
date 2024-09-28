using HotChocolate;
using HotChocolate.Authorization; 
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Services;
using backend.DTOs;
using System.Security.Claims;

namespace backend.GraphQL{

public class UserQuery
{
    private readonly IUserService _userService; 
    public UserQuery(IUserService userService)
    {
        _userService = userService;
    }

    [GraphQLName("user")]
    public async Task<UserDto?> GetUserProfile([Service] IUserService userService, ClaimsPrincipal claimsPrincipal)
    {
        var auth0Id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0Id == null)
        {
            throw new Exception("Invalid token.");
        }

        return await userService.GetUserProfileAsync(auth0Id);
    }
}

}
