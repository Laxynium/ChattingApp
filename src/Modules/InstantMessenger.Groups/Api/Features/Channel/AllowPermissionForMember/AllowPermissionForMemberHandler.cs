using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.AllowPermissionForMember
{
    internal sealed class AllowPermissionForMemberHandler : ICommandHandler<AllowPermissionForMemberCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AllowPermissionForMemberHandler(IGroupRepository groupRepository, IChannelRepository channelRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AllowPermissionForMemberCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ??
                        throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) ??
                          throw new ChannelNotFoundException(ChannelId.From(command.ChannelId));

            group.AllowPermission(
                UserId.From(command.UserId),
                channel,
                UserId.From(command.MemberUserId),
                Permission.FromName(command.Permission)
            );


            await _unitOfWork.Commit();
        }
    }
}