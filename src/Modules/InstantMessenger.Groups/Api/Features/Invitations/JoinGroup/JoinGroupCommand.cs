﻿using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Events;
using NodaTime;

namespace InstantMessenger.Groups.Api.Features.Invitations.JoinGroup
{
    public class JoinGroupCommand : ICommand
    {
        public Guid UserId { get; }
        public string Code { get; }

        public JoinGroupCommand(Guid userId, string code)
        {
            UserId = userId;
            Code = code;
        }
    }

    internal sealed class JoinGroupHandler : ICommandHandler<JoinGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;
        private readonly IEventDispatcher _eventDispatcher;

        public JoinGroupHandler(IGroupRepository groupRepository, IInvitationRepository invitationRepository, IUnitOfWork unitOfWork, IClock clock,
            IEventDispatcher eventDispatcher)
        {
            _groupRepository = groupRepository;
            _invitationRepository = invitationRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
            _eventDispatcher = eventDispatcher;
        }
        public async Task HandleAsync(JoinGroupCommand command)
        {
            var invitationCode = InvitationCode.From(command.Code);
            var invitation = await _invitationRepository.GetAsync(invitationCode) ?? throw new InvalidInvitationCodeException(command.Code);
            var group = await _groupRepository.GetAsync(invitation.GroupId) ?? throw new GroupNotFoundException(invitation.GroupId);

            invitation.Use(UserId.From(command.UserId), group, _clock);

            await _unitOfWork.Commit();

            await _eventDispatcher.PublishAsync(new InvitationUsedEvent(command.UserId, invitation.Id.Value, invitation.GroupId.Value));
        }
    }
}