using ETicaretAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

 
        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach(string to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Username"], "Mini E-Ticaret", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = int.Parse(_configuration["Mail:Port"]);
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetAsync(string to,string userId, string resetToken)
        {
            StringBuilder mail = new StringBuilder();
            mail.AppendLine("Hello<br/>If you want a new password, you can continue from this link to change your password" +
                "<strong><a target = \" _blank \" href =\"");
            mail.AppendLine(_configuration["AngularClientUrl"]);
            mail.AppendLine("/password-update/");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Click for new password</a></strong><br/><br/><span>If this process is not made by you, you do not have to do anything</span>" +
                "<br/><br/>Mini E-Ticaret");

            await SendMailAsync(to, "New Password Process", mail.ToString());

        }

        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName)
        {
            string mail = $"Dear {userName}, <br>" +
                $"Your order : {orderCode} is accepted at {orderDate} and send for transition.<br> It is an honor to serve you.";

            await SendMailAsync(to, $"Your Purchasing Information for {orderCode} ",mail);
        }
    }
}
