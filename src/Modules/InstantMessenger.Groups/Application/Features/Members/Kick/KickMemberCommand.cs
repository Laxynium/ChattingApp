using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Members.Kick
{
    public class KickMemberCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid UserIdOfMember { get; }

        public KickMemberCommand(Guid userId, Guid groupId, Guid userIdOfMember)
        {
            UserId = userId;
            GroupId = groupId;
            UserIdOfMember = userIdOfMember;
        }
    }

    internal sealed class KickMemberHandler : ICommandHandler<KickMemberCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public KickMemberHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(KickMemberCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.KickMember(UserId.From(command.UserId), UserId.From(command.UserIdOfMember));
        }
    }
}