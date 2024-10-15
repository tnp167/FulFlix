using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using Microsoft.Extensions.Options;
using Resend;

namespace backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly ResendClient _resendClient;

        public EmailService(ResendClient resendClient)
        {
            _resendClient = resendClient;
        }

        public async Task SendVerificationEmail(string email, string verificationLink)
        {
            var message = new EmailMessage
            {
                From = "onboarding@resend.dev",
                To = email,
                Subject = "FulFlix - Confirmation Email",
                HtmlBody = $"<p>Click to <a href=\"{verificationLink}\">confirm your email</a></p>",
            };

            await _resendClient.EmailSendAsync(message);
        }
    }
}
