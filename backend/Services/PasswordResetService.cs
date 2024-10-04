using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using backend.Interfaces;
using Resend;
using Newtonsoft.Json;
using System.Text;

namespace backend.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly HttpClient _httpClient;
        private readonly IResend _resend;

        private readonly IAuth0Client _auth0Client;
        public PasswordResetService(HttpClient httpClient, IResend resend, IAuth0Client auth0Client)
        {
            _httpClient = httpClient;
            _resend = resend;
            _auth0Client = auth0Client;
        }

        public async Task<bool> UpdateUserPassword(string newPassword, string auth0Id)
        {
            var auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN")!;
            var auth0Token = await _auth0Client.GetManagementApiTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Patch, $"https://{auth0Domain}/api/v2/users/{auth0Id}");
            request.Headers.Add("Authorization", $"Bearer {auth0Token}");

            var body = new
            {
                password = newPassword,
                connection = "Username-Password-Authentication"
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task SendPasswordResetEmail(string email, string resetLink){
            var message = new EmailMessage();
            message.From = "onboarding@resend.dev";
            message.To.Add(email);
            message.Subject = "FulFlix - Reset Password Token";
            message.HtmlBody = $"<p> Click to <a href=\"{resetLink}\">reset your password</a></p>";

            await _resend.EmailSendAsync(message);
        }
    }
}