using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Channel.AddChannel
{
    internal sealed class CreateChannelHandler : ICommandHandler<CreateChannelCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;

        public CreateChannelHandler(IGroupRepository groupRepository, IChannelRepository channelRepository)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
        }
        public async Task HandleAsync(CreateChannelCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            if (await _channelRepository.ExistsAsync(group.Id, ChannelId.From(command.ChannelId)))
                throw new ChannelAlreadyExistsException(command.ChannelId);

            var channel = group.CreateChannel(UserId.From(command.UserId), ChannelId.From(command.ChannelId), ChannelName.Create(command.ChannelName));

            await _channelRepository.AddAsync(channel);
        }
    }
}