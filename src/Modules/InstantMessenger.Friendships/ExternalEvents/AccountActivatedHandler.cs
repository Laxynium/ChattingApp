using System.Threading.Tasks;
using InstantMessenger.Friendships.Api;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Friendships.ExternalEvents
{
    internal class AccountActivatedHandler : IEventHandler<AccountActivatedEvent>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountActivatedHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(AccountActivatedEvent @event)
        {
            if (await _personRepository.ExistsAsync(@event.UserId))
            {
                return;
            }
            var person = new Person(@event.UserId);

            await _personRepository.AddAsync(person);

            await _unitOfWork.Commit();
        }
    }
}