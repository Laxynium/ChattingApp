using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Messages;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;
using NodaTime;

namespace InstantMessenger.Groups.Application.Features.Messages.SendMessage
{
    public class SendMessageCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid MessageId { get; }
        public string Content { get; }

        public SendMessageCommand(Guid userId, Guid groupId, Guid channelId, Guid messageId, string content)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            MessageId = messageId;
            Content = content;
        }
    }

    internal sealed class SendMessageHandler : ICommandHandler<SendMessageCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IClock _clock;

        public SendMessageHandler(IGroupRepository groupRepository, IChannelRepository channelRepository,IMessageRepository messageRepository, IClock clock)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
            _messageRepository = messageRepository;
            _clock = clock;
        }

        public async Task HandleAsync(SendMessageCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) ?? throw new ChannelNotFoundException(ChannelId.From(command.ChannelId));

            if (await _messageRepository.ExistsAsync(ChannelId.From(command.ChannelId), MessageId.From(command.MessageId)))
            {
                throw new MessageAlreadyExistsException(command.MessageId);
            }

            var message = group.SendMessage(UserId.From(command.UserId), channel, MessageId.From(command.MessageId), MessageContent.Create(command.Content), _clock);

            await _messageRepository.AddAsync(message);
        }
    }
}