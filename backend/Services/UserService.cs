using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Repositories;
using backend.DTOs;
using backend.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuth0Client _auth0Client;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper, IAuth0Client auth0Client)
        {
            _userRepository = userRepository;
            _auth0Client = auth0Client;
            _mapper = mapper;
        }

        //Sign up
        public async Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var userMetadata = new Auth0UserMetadataDto
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName
            };

            // Create the Auth0 user request
            var auth0UserResponse = await _auth0Client.CreateAuth0UserAsync(new Auth0UserRequestDto
            {
                Email = createUserDto.Email,
                Password = createUserDto.Password,
                UserMetadata = userMetadata 
            });

            Console.WriteLine(JsonConvert.SerializeObject(auth0UserResponse, Formatting.Indented));

            if (auth0UserResponse == null)
            {
                throw new Exception("Failed to create user in Auth0.");
            }

            //Assign User role to Auth0 user
            bool roleAssigned = await _auth0Client.AssignRoleToUserAsync(auth0UserResponse.UserId,"User");

            if (!roleAssigned)
            {
                throw new Exception("Failed to assign role to user in Auth0.");
            }
            
            //Store user information in local database
            var newUser = new User
            {
                Auth0Id = auth0UserResponse.UserId,
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                EmailVerified = false,
                Picture = auth0UserResponse.Picture,
                Roles = new List<Role> { Role.User },
                Location = createUserDto.Location, 
                BirthDate = createUserDto.BirthDate,
                Phone = createUserDto.Phone,
                PrivacyConsent = createUserDto.PrivacyConsent,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _userRepository.CreateUserAsync(newUser);

            return _mapper.Map<UserDto>(newUser);
        }

        //Login 
        public async Task<string?> LoginUserAsync (LoginUserDto loginUserDto){

            var token = await _auth0Client.LoginAuth0UserAsync(loginUserDto.Email, loginUserDto.Password);

            if (token == null)
            {
                throw new Exception("Invalid Credentials");
            }

            return token;
        }

       //Get User
       public async Task<UserDto?> GetUserProfileAsync(string auth0Id)
       {
            var auth0user = await _userRepository.GetUserByAuth0IdAsync(auth0Id);
            if (auth0user == null)
            {
                throw new Exception("User not found.");
            }

            return _mapper.Map<UserDto>(auth0user);
      }
    }
}