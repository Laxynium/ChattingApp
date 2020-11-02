using System.Threading.Tasks;
using MimeKit;

namespace InstantMessenger.Shared.MailKit
{
    public interface IMailService
    {
        Task SendAsync(MimeMessage message);
    }
}