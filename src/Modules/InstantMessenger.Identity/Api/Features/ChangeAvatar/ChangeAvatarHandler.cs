using System.Threading.Tasks;
using InstantMessenger.Identity.Api.Features.ChangeNickname;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Identity.Api.Features.ChangeAvatar
{
    internal sealed class ChangeAvatarHandler : ICommandHandler<ChangeAvatarCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeAvatarHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(ChangeAvatarCommand command)
        {
            var user = await _userRepository.GetAsync(command.UserId)?? throw new UserNotFoundException();

            var avatar = await Avatar.Create(command.Image);
            user.Change(avatar);

            await _unitOfWork.Commit();
        }
    }
}