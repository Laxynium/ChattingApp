using System.Threading.Tasks;
using InstantMessenger.Profiles.Domain;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Profiles.Api.Features.NicknameChange
{
    internal sealed class ChangeNicknameHandler : ICommandHandler<ChangeNicknameCommand>
    {
        private readonly IProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeNicknameHandler(IProfileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(ChangeNicknameCommand command)
        {
            var profile = await _repository.GetAsync(command.ProfileId) ?? throw new ProfileNotFoundException();

            profile.Change(Nickname.Create(command.Nickname));

            await _unitOfWork.Commit();
        }
    }
}