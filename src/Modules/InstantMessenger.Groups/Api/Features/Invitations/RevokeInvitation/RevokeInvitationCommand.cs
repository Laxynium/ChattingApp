using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Invitations.RevokeInvitation
{
    public class RevokeInvitationCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid InvitationId { get; }

        public RevokeInvitationCommand(Guid userId, Guid groupId, Guid invitationId)
        {
            UserId = userId;
            GroupId = groupId;
            InvitationId = invitationId;
        }
    }

    internal sealed class RevokeInvitationCommandHandler : ICommandHandler<RevokeInvitationCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IInvitationRepository _invitationRepository;

        public RevokeInvitationCommandHandler(IGroupRepository groupRepository, IInvitationRepository invitationRepository)
        {
            _groupRepository = groupRepository;
            _invitationRepository = invitationRepository;
        }
        public async Task HandleAsync(RevokeInvitationCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ??
                        throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var invitation = await _invitationRepository.GetAsync(InvitationId.From(command.InvitationId));

            group.RevokeInvitation(UserId.From(command.UserId), invitation);

            await _invitationRepository.RemoveAsync(invitation);
        }
    }
}