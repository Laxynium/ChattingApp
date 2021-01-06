using System.Threading.Tasks;
using InstantMessenger.Identity.Application.IntegrationEvents;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.MailKit;
using InstantMessenger.Shared.MailKit.Builders;
using MimeKit;

namespace InstantMessenger.Identity.Application.Features.SignUp.EventHandlers
{
    public class ActivationLinkCreatedEventHandler : IIntegrationEventHandler<ActivationLinkCreatedEvent>
    {
        private readonly LinkGenerator _generator;
        private readonly IMailService _mailService;
        private readonly MailOptions _mailOptions;
        private readonly IUserRepository _userRepository;

        public ActivationLinkCreatedEventHandler(LinkGenerator generator, IMailService mailService, MailOptions mailOptions, IUserRepository userRepository)
        {
            _generator = generator;
            _mailService = mailService;
            _mailOptions = mailOptions;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(ActivationLinkCreatedEvent @event)
        {
            var user = await _userRepository.GetAsync(new UserId(@event.UserId));
            var url = _generator.GenerateActivationLink(user.Id, @event.Token);
            var message = BuildMessage(user, url);

            await _mailService.SendAsync(message);
        }

        private MimeMessage BuildMessage(User user, string url) => IMessageBuilder.Create()
            .WithSender(_mailOptions.FromEmail)
            .WithReceiver(user.Email)
            .WithSubject(MailTemplates.AccountVerification.Subject)
            .WithBody(MailTemplates.AccountVerification.Body, url)
            .Build();
    }
}