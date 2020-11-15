using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;
using NodaTime;

namespace InstantMessenger.Groups.Api.Features.Members.Add
{
    public class AddGroupMember : ICommand
    {
        public Guid UserId { get; }
        public Guid NewMemberUserId { get; }
        public Guid GroupId { get; }
        public string MemberName { get; }

        public AddGroupMember(Guid userId, Guid newMemberUserId, Guid groupId, string memberName)
        {
            UserId = userId;
            NewMemberUserId = newMemberUserId;
            GroupId = groupId;
            MemberName = memberName;
        }
    }

    internal sealed class AddGroupHandler : ICommandHandler<AddGroupMember>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public AddGroupHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork, IClock clock)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }
        public async Task HandleAsync(AddGroupMember command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException();

            group.AddMember(UserId.From(command.UserId),UserId.From(command.NewMemberUserId), MemberName.Create(command.MemberName), _clock);

            await _unitOfWork.Commit();
        }
    }
}