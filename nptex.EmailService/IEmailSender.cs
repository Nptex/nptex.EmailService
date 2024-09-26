using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nptex.EmailService
{

    public interface IEmailSender
    {
        Task SendPlainEmailAsync(string to, string subject, string body);
        Task SendEmailWithAttachmentAsync(string to, string subject, string body, List<string> attachments);
    }
}