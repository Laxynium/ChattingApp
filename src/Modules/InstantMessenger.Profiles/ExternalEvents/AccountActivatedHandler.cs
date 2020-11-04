using System.Threading.Tasks;
using InstantMessenger.Profiles.Domain;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Profiles.ExternalEvents
{
    public class AccountActivatedHandler : IEventHandler<AccountActivatedEvent>
    {
        private readonly IProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountActivatedHandler(IProfileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(AccountActivatedEvent @event)
        {
            if (await _repository.ExistsAsync(@event.UserId))
            {
                return;
            }

            var profile = new Profile(@event.UserId);

            await _repository.AddAsync(profile);

            await _unitOfWork.Commit();
        }
    }
}