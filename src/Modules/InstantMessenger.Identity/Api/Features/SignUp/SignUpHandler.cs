using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MailKit;
using InstantMessenger.Shared.MailKit.Builders;
using Microsoft.AspNetCore.Identity;
using MimeKit;

namespace InstantMessenger.Identity.Api.Features.SignUp
{
    internal sealed class SignUpHandler : ICommandHandler<SignUpCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IActivationLinkRepository _activationLinkRepository;
        private readonly IUniqueEmailRule _uniqueEmailRule;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RandomStringGenerator _stringGenerator;
        private readonly LinkGenerator _generator;
        private readonly IMailService _mailService;
        private readonly MailOptions _mailOptions;

        public SignUpHandler(
            IUserRepository userRepository,
            IActivationLinkRepository activationLinkRepository,
            IUniqueEmailRule uniqueEmailRule, 
            IPasswordHasher<User> passwordHasher,
            RandomStringGenerator stringGenerator,
            LinkGenerator generator,
            IMailService mailService,
            MailOptions mailOptions)
        {
            _userRepository = userRepository;
            _activationLinkRepository = activationLinkRepository;
            _uniqueEmailRule = uniqueEmailRule;
            _passwordHasher = passwordHasher;
            _stringGenerator = stringGenerator;
            _generator = generator;
            _mailService = mailService;
            _mailOptions = mailOptions;
        }
        public async Task HandleAsync(SignUpCommand request)
        {
            var user = await User.Create(Email.Create(request.Email), Password.Create(request.Password), _uniqueEmailRule, _passwordHasher);
            
            await _userRepository.AddAsync(user);

            var randomString = _stringGenerator.Generate(30);

            var activationLink = ActivationLink.Create(user.Id, randomString);

            await _activationLinkRepository.AddAsync(activationLink);

            var url = _generator.GenerateActivationLink(user.Id, randomString);

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