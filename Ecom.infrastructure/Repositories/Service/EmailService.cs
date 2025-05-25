using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTO;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Ecom.infrastructure.Repositories.Service
{
    internal class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(EmailDTO emailDTO)
        {
             MimeMessage message = new MimeMessage();
            message.From.Add (new MailboxAddress( "Nawras",configuration["EmailSettings:From"]));
            message.Subject= emailDTO.Subject;
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };
            using(var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try{
                   await smtp.ConnectAsync(
                       configuration["EmailSettings:Smtp"],
                          int.Parse(configuration["EmailSettings:Port"]),true
                       );
                    await smtp.AuthenticateAsync(
                        configuration["EmailSettings:UserName"],
                        configuration["EmailSettings:Password"]
                        );
                    await smtp.SendAsync(message);
                }
                catch(Exception ex)    
                {
                    throw;
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                    smtp.Dispose();

                }
            }

        }
    }
}
