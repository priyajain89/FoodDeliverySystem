using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.Domain.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Services
{
    public class EmailService{

        private readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient(_settings.SmtpHost)
            {
                Port = _settings.SmtpPort,
                EnableSsl = true,
                Credentials = new NetworkCredential(_settings.FromEmail, _settings.AppPassword)
            };
        }
        public void SendSimpleEmail(string toEmail, string subject, string body)
        {
            var message = new MailMessage(_settings.FromEmail, toEmail, subject, body);
            CreateSmtpClient().Send(message);
        }


    }
}
