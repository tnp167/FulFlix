using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs;

namespace backend.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto);
        Task<string?> LoginUserAsync (LoginUserDto loginUserDto);
    }
}