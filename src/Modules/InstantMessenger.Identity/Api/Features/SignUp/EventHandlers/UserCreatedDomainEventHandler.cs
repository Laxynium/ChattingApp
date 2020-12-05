using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Events;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Identity.Api.Features.SignUp.EventHandlers
{
    internal sealed class UserCreatedEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
    {
        private readonly IActivationLinkRepository _activationLinkRepository;
        private readonly RandomStringGenerator _stringGenerator;

        public UserCreatedEventHandler(IActivationLinkRepository activationLinkRepository, RandomStringGenerator stringGenerator)
        {
            _activationLinkRepository = activationLinkRepository;
            _stringGenerator = stringGenerator;
        }
        public async Task HandleAsync(UserCreatedDomainEvent @event)
        {
            var randomString = _stringGenerator.Generate(30);

            var activationLink = ActivationLink.Create(@event.Id, randomString);

            await _activationLinkRepository.AddAsync(activationLink);
        }
    }
}