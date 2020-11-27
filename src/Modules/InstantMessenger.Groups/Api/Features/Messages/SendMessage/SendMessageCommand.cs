﻿using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Messages;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;
using NodaTime;

namespace InstantMessenger.Groups.Api.Features.Messages.SendMessage
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public SendMessageHandler(IGroupRepository groupRepository, IChannelRepository channelRepository,IMessageRepository messageRepository, IUnitOfWork unitOfWork, IClock clock)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public async Task HandleAsync(SendMessageCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) ?? throw new ChannelNotFoundException(ChannelId.From(command.ChannelId));

            var message = group.SendMessage(UserId.From(command.UserId), channel, MessageId.From(command.MessageId), new MessageContent(command.Content), _clock);

            await _messageRepository.AddAsync(message);
            await _unitOfWork.Commit();
        }
    }
}