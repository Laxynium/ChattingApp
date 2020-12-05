using System.Threading.Tasks;
using InstantMessenger.Identity.Api.Features.ChangeNickname;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.BuildingBlocks;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Identity.Api.Features.ChangeAvatar
{
    internal sealed class ChangeAvatarHandler : ICommandHandler<ChangeAvatarCommand>
    {
        private readonly IUserRepository _userRepository;

        public ChangeAvatarHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task HandleAsync(ChangeAvatarCommand command)
        {
            var user = await _userRepository.GetAsync(new UserId(command.UserId))?? throw new UserNotFoundException();

            var avatar = await Avatar.Create(command.Image);
            user.Change(avatar);
        }
    }
}