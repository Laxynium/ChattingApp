using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Shared.MailKit;
using MimeKit;

namespace InstantMessenger.IntegrationTests
{
    public class FakeMailService : IMailService
    {
        private readonly List<MimeMessage> _messages = new List<MimeMessage>();
        public IReadOnlyList<MimeMessage> Messages => _messages.AsReadOnly();
        public Task SendAsync(MimeMessage message)
        {
            _messages.Add(message);
            return Task.CompletedTask;;
        }
    }
}