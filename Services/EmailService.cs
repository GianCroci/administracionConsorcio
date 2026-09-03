using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _fromEmail = configuration["SendGrid:FromEmail"];
            _fromName = configuration["SendGrid:FromName"];
            _logger = logger;
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

            var response = await client.SendEmailAsync(msg);

            var body = await response.Body.ReadAsStringAsync();
            _logger.LogInformation("SendGrid status: {Status}, body: {Body}", response.StatusCode, body);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"SendGrid respondió {response.StatusCode}: {body}");
            }
        }
    }
}