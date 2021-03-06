﻿using System.Threading.Tasks;
using InstantMessenger.Identity.Application.Features.SignIn;
using InstantMessenger.Identity.Application.Features.SignUp;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.MailKit;
using InstantMessenger.Shared.MailKit.Builders;
using InstantMessenger.Shared.Messages.Commands;
using MimeKit;

namespace InstantMessenger.Identity.Application.Features.SendPasswordReset
{
    internal sealed class SendPasswordResetHandler : ICommandHandler<SendPasswordResetCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMailService _mailService;
        private readonly MailOptions _mailOptions;
        private readonly LinkGenerator _linkGenerator;

        public SendPasswordResetHandler(IUserRepository userRepository,
            IAuthTokenService authTokenService,
            IMailService mailService,
            MailOptions mailOptions,
            LinkGenerator linkGenerator)
        {
            _userRepository = userRepository;
            _authTokenService = authTokenService;
            _mailService = mailService;
            _mailOptions = mailOptions;
            _linkGenerator = linkGenerator;
        }
        public async Task HandleAsync(SendPasswordResetCommand command)
        {
            var user = await _userRepository.GetAsync(command.Email);
            if (user is null)
            {
                return;
            }

            var token = _authTokenService.Create(user.Id, user.PasswordHash);
            var link = _linkGenerator.GenerateResetPasswordLink(user.Id,token);

            var message = BuildMessage(user, link);

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