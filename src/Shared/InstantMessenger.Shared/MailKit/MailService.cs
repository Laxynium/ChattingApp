using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InstantMessenger.Shared.MailKit
{

    internal sealed class MailService : IMailService
    {
        private readonly ILogger<IMailService> _logger;
        private readonly MailKitOptions _mailKitOptions;

        public MailService(IOptions<MailKitOptions> mailOptions, ILogger<IMailService> logger)
        {
            _logger = logger;
            _mailKitOptions = mailOptions.Value;
        }
        public async Task SendAsync(MimeMessage message)
        {
            using var scope = _logger.BeginScope("Mail sending");
            using var client = new SmtpClient();
            _logger.LogInformation("Trying to connected with smtp server ...");
            await client.ConnectAsync(_mailKitOptions.Host,_mailKitOptions.Port, _mailKitOptions.UseSsl);
            
            if (_mailKitOptions.Authenticate)
            {
                await client.AuthenticateAsync(_mailKitOptions.Username, _mailKitOptions.Password);
            }
            _logger.LogInformation("Connected with smtp server.");
            _logger.LogInformation($"Trying to send message [Id: {message.MessageId}, To: {string.Join(";",message.To)}]...");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            _logger.LogInformation($"Message [Id: {message.MessageId}, To: {string.Join(";",message.To)}] has been sent.");
        }
    }
}