using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using backend.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resend;

namespace backend.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ResendClient _resendClient;
        private readonly IAuth0Client _auth0Client;

        public PasswordResetService(
            IHttpClientFactory httpClientFactory,
            ResendClient resendClient,
            IAuth0Client auth0Client
        )
        {
            _httpClientFactory = httpClientFactory;
            _resendClient = resendClient;
            _auth0Client = auth0Client;
        }

        public async Task<bool> UpdateUserPassword(string newPassword, string auth0Id)
        {
            var auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN")!;
            var auth0Token = await _auth0Client.GetManagementApiTokenAsync();

            using var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(
                HttpMethod.Patch,
                $"https://{auth0Domain}/api/v2/users/{auth0Id}"
            );
            request.Headers.Add("Authorization", $"Bearer {auth0Token}");

            var body = new
            {
                password = newPassword,
                connection = "Username-Password-Authentication",
            };

            request.Content = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task SendPasswordResetEmail(string email, string resetLink)
        {
            var message = new EmailMessage
            {
                From = "your-verified-sender@example.com", // Use a verified sender email
                To = email,
                Subject = "FulFlix - Reset Password Token",
                HtmlBody = $"<p>Click to <a href=\"{resetLink}\">reset your password</a></p>",
            };

            try
            {
                await _resendClient.EmailSendAsync(message);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error sending password reset email: {ex}");
                throw;
            }
        }
    }
}
