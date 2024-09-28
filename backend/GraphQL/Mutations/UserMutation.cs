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
    public class UserMutation
    {
         private readonly IUserService _userService;
         public UserMutation(IUserService userService)
         {
            _userService = userService;  
         }

        public async Task<UserDto?> CreateUser(CreateUserDto createUserDto)
        {
            return await _userService.CreateUserAsync(createUserDto);
        }

        public async Task<string?> LoginUser(LoginUserDto loginUserDto)
        {
            return  await _userService.LoginUserAsync(loginUserDto);
        }
    }
}