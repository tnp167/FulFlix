using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Interfaces;
using Newtonsoft.Json;

namespace backend.Services
{
    public class Auth0Client : IAuth0Client
    {
        private readonly HttpClient _httpClient;
        private readonly string _auth0Domain;

        public Auth0Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN")!;
            _httpClient.BaseAddress = new Uri($"https://{_auth0Domain}/");
        }

        public async Task<Auth0UserResponseDto?> CreateAuth0UserAsync(Auth0UserRequestDto request)
        {
            var token = await GetManagementApiTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            var response = await _httpClient.PostAsync(
                "api/v2/users",
                new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Auth0UserResponseDto>(responseContent);
        }

        public async Task<string> GetManagementApiTokenAsync()
        {
            var clientId = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID")!;
            var audience = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE")!;
            var clientSecret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET")!;

            var requestData = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "audience", audience },
            };

            var content = new FormUrlEncodedContent(requestData);
            var response = await _httpClient.PostAsync("oauth/token", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error fetching Management API token: {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<Auth0TokenResponse>(responseJson);

            return tokenResponse?.AccessToken
                ?? throw new Exception("Failed to retrieve access token");
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            var token = await GetManagementApiTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            var roleId = await GetRoleIdByNameAsync(_httpClient, roleName);

            if (roleId == null)
            {
                throw new Exception($"Role '{roleName}' not found in Auth0.");
            }

            var content = new StringContent(
                JsonConvert.SerializeObject(new { roles = new[] { roleId } }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"api/v2/users/{userId}/roles", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to assign role in Auth0: {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }

        private async Task<string?> GetRoleIdByNameAsync(HttpClient client, string roleName)
        {
            var response = await client.GetAsync("api/v2/roles");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error fetching roles from Auth0: {errorContent}");
            }

            var rolesJson = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<Auth0RoleDto>>(rolesJson);

            var role = roles?.FirstOrDefault(r =>
                r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase)
            );
            return role?.Id;
        }

        public async Task<string?> LoginAuth0UserAsync(string email, string password)
        {
            var clientId = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID")!;
            var audience = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE")!;
            var clientSecret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET")!;

            var requestData = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", email },
                { "password", password },
                { "audience", audience },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "connection", "Username-Password-Authentication" },
            };

            var content = new FormUrlEncodedContent(requestData);
            var response = await _httpClient.PostAsync("oauth/token", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error during login: {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<Auth0TokenResponse>(responseJson);

            return tokenResponse?.AccessToken;
        }
    }
}
