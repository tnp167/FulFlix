using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using AutoMapper;
using backend.DTOs;

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
            return await _userService.CreateUserAsync(createUserDto);
        }

        [GraphQLName("loginUser")]
        public async Task<string?> LoginUser(LoginUserDto loginUserDto)
        {
            return  await _userService.LoginUserAsync(loginUserDto);
        }
    }
}