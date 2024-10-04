using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Repositories;
using backend.DTOs;
using backend.Models;
using Resend;
using AutoMapper;

namespace backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        public EmailService(IResend resend)
        {
            _resend = resend;
        }
        public async Task SendVerificationEmail(string email, string verificationLink){
            var message = new EmailMessage();
            message.From = "onboarding@resend.dev";
            message.To.Add(email);
            message.Subject = "FulFlix - Confirmation Email";
            message.HtmlBody = $"<p> Click to <a href=\"{verificationLink}\">confirm your email</a></p>";

            await _resend.EmailSendAsync(message);
        }

    }
}