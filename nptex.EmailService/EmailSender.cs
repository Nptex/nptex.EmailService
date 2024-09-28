using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace nptex.EmailService
{

        public class EmailSender : IEmailSender
        {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendPlainEmailAsync(string to, string subject, string body)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailConfig.From, _emailConfig.From));
                message.To.Add(new MailboxAddress(to, to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }

            public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, List<string> attachments)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailConfig.From, _emailConfig.From));
                message.To.Add(new MailboxAddress(to, to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = body;

                foreach (var attachment in attachments)
                {
                    if (File.Exists(attachment))
                    {
                        bodyBuilder.Attachments.Add(attachment);
                    }
                }

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
        }
    
}