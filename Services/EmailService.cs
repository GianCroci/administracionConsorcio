using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class EmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _fromEmail = configuration["SendGrid:FromEmail"];
            _fromName = configuration["SendGrid:FromName"];
        }

        public async Task EnviarAsync(List<string> destinatarios, string asunto, string cuerpoHtml)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var msg = new SendGridMessage();

            msg.SetFrom(from);
            msg.SetSubject(asunto);
            msg.AddContent(MimeType.Html, cuerpoHtml);

            foreach (var email in destinatarios)
                msg.AddTo(new EmailAddress(email));

            await client.SendEmailAsync(msg);
        }
    }
}