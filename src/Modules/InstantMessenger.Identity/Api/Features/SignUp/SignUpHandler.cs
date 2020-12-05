using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Shared.MailKit;
using InstantMessenger.Shared.MailKit.Builders;
using InstantMessenger.Shared.Messages.Commands;
using Microsoft.AspNetCore.Identity;
using MimeKit;

namespace InstantMessenger.Identity.Api.Features.SignUp
{
    internal sealed class SignUpHandler : ICommandHandler<SignUpCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUniqueEmailRule _uniqueEmailRule;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly MailOptions _mailOptions;

        public SignUpHandler(
            IUserRepository userRepository,
            IUniqueEmailRule uniqueEmailRule, 
            IPasswordHasher<User> passwordHasher,
            MailOptions mailOptions)
        {
            _userRepository = userRepository;
            _uniqueEmailRule = uniqueEmailRule;
            _passwordHasher = passwordHasher;
            _mailOptions = mailOptions;
        }
        public async Task HandleAsync(SignUpCommand request)
        {
            var user = await User.Create(Email.Create(request.Email), Password.Create(request.Password), _uniqueEmailRule, _passwordHasher);
            
            await _userRepository.AddAsync(user);
        }

        private MimeMessage BuildMessage(User user, string url) => IMessageBuilder.Create()
            .WithSender(_mailOptions.FromEmail)
            .WithReceiver(user.Email)
            .WithSubject(MailTemplates.AccountVerification.Subject)
            .WithBody(MailTemplates.AccountVerification.Body, url)
            .Build();
    }
}