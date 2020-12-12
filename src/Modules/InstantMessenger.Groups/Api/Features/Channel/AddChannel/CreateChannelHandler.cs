using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.AddChannel
{
    internal sealed class CreateChannelHandler : ICommandHandler<CreateChannelCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateChannelHandler(IGroupRepository groupRepository, IChannelRepository channelRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(CreateChannelCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            if (await _channelRepository.ExistsAsync(group.Id, ChannelId.From(command.ChannelId)))
                throw new ChannelAlreadyExistsException(command.ChannelId);

            var channel = group.CreateChannel(UserId.From(command.UserId), ChannelId.From(command.ChannelId), ChannelName.Create(command.ChannelName));

            await _channelRepository.AddAsync(channel);
            await _unitOfWork.Commit();
        }
    }
}