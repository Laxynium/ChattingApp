using System.Threading.Tasks;
using InstantMessenger.Identity.Api.Features.SignIn;
using InstantMessenger.Identity.Api.Features.SignUp;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MailKit;
using InstantMessenger.Shared.MailKit.Builders;
using MimeKit;

namespace InstantMessenger.Identity.Api.Features.SendPasswordReset
{
    internal sealed class SendPasswordResetHandler : ICommandHandler<SendPasswordResetCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMailService _mailService;
        private readonly MailOptions _mailOptions;

        public SendPasswordResetHandler(IUserRepository userRepository,
            IAuthTokenService authTokenService,
            IMailService mailService,
            MailOptions mailOptions)
        {
            _userRepository = userRepository;
            _authTokenService = authTokenService;
            _mailService = mailService;
            _mailOptions = mailOptions;
        }
        public async Task HandleAsync(SendPasswordResetCommand command)
        {
            var user = await _userRepository.GetAsync(command.Email);
            if (user is null)
            {
                return;
            }

            var token = _authTokenService.Create(user.Id, user.PasswordHash);

            var message = BuildMessage(user, token);

            await _mailService.SendAsync(message);
        }

        private MimeMessage BuildMessage(User user, string token) => IMessageBuilder.Create()
            .WithSender(_mailOptions.FromEmail)
            .WithReceiver(user.Email)
            .WithSubject(MailTemplates.ResetPassword.Subject)
            .WithBody(MailTemplates.ResetPassword.Body, token)
            .Build();
    }
}