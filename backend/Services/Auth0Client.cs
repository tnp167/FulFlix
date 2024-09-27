using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace backend.Services
{
    public class Auth0Client : IAuth0Client
    {
         private readonly HttpClient _httpClient;

         public Auth0Client(HttpClient httpClient)
         {
            _httpClient = httpClient;
         }

        public async Task<Auth0UserResponseDto?> CreateAuth0UserAsync(Auth0UserRequestDto request)
        {
            var response = await _httpClient.PostAsync("api/v2/users", 
                new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
            
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var auth0UserResponse = JsonConvert.DeserializeObject<Auth0UserResponseDto>(responseContent);

            return auth0UserResponse;
         }

    public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
    {
        var roleId = await GetRoleIdByNameAsync(roleName);
        
        if (roleId == null)
        {
            throw new Exception($"Role '{roleName}' not found in Auth0.");
        }

        var content = new StringContent(JsonConvert.SerializeObject(new { roles = new[] { roleId } }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"api/v2/users/{userId}/roles", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to assign role in Auth0: {errorContent}");
        }

        return response.IsSuccessStatusCode;
    }

    private async Task<string?> GetRoleIdByNameAsync(string roleName)
    {
        var response = await _httpClient.GetAsync("api/v2/roles");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error fetching roles from Auth0: {errorContent}");
        }

        var rolesJson = await response.Content.ReadAsStringAsync();
        var roles = JsonConvert.DeserializeObject<List<Auth0RoleDto>>(rolesJson);

        var role = roles?.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        return role?.Id;
    }


    }
}