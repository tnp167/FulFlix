using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.DTOs;
using backend.Interfaces;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;

namespace backend.GraphQL
{
    [ExtendObjectType("Mutation")]
    public class UserMutation
    {
        private readonly IUserService _userService;

        public UserMutation(IUserService userService)
        {
            _userService = userService;
        }

        [GraphQLName("createUser")]
        public async Task<UserDto?> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                return await _userService.CreateUserAsync(createUserDto);
            }
            catch (Exception ex)
            {
                throw new GraphQLException(new Error(ex.Message, "USER_CREATION_ERROR"));
            }
        }

        [GraphQLName("loginUser")]
        public async Task<string?> LoginUser(LoginUserDto loginUserDto)
        {
            return await _userService.LoginUserAsync(loginUserDto);
        }
    }
}
