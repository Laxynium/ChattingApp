using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Members.Kick
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
        private readonly IUnitOfWork _unitOfWork;

        public KickMemberHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(KickMemberCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.KickMember(UserId.From(command.UserId), UserId.From(command.UserIdOfMember));

            await _unitOfWork.Commit();
        }
    }
}