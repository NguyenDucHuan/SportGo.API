using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.Services.Interfaces
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
        Task SendEmailWithTemplateAsync(string toEmail, string subject, string templateName, Dictionary<string, string> replacements);
    }
}
