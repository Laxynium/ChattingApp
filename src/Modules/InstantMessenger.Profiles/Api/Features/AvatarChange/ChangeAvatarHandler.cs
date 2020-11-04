using System.Threading.Tasks;
using InstantMessenger.Profiles.Domain;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Profiles.Api.Features.AvatarChange
{
    internal sealed class ChangeAvatarHandler : ICommandHandler<ChangeAvatarCommand>
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeAvatarHandler(IProfileRepository profileRepository, IUnitOfWork unitOfWork)
        {
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(ChangeAvatarCommand command)
        {
            var profile = await _profileRepository.GetAsync(command.ProfileId);


            var avatar = await Avatar.Create(command.Image);
            profile.Change(avatar);

            await _unitOfWork.Commit();
        }
    }
}