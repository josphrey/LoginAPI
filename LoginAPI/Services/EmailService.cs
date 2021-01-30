using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using LoginAPI.Helpers;

namespace LoginAPI.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        //private readonly AppSettings _appSettings;

        //public EmailService(IOptions<AppSettings> appSettings)
        //{
        //    _appSettings = appSettings.Value;
        //}

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? "aditya.green82@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("aditya.green82@ethereal.email", "GjKQAnX31yTBxFEP2H");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
