using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Features.Group.Create.ExternalQueries;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Modules;
using NodaTime;

namespace InstantMessenger.Groups.Application.Features.Members.Add
{
    public class AddGroupMember : ICommand
    {
        public Guid GroupId { get; }
        public Guid UserIdOfMember { get; }

        public AddGroupMember(Guid groupId, Guid userIdOfMember)
        {
            GroupId = groupId;
            UserIdOfMember = userIdOfMember;
        }
    }

    internal sealed class AddGroupHandler : ICommandHandler<AddGroupMember>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IModuleClient _moduleClient;
        private readonly IClock _clock;

        public AddGroupHandler(IGroupRepository groupRepository, 
            IModuleClient moduleClient,
            IClock clock)
        {
            _groupRepository = groupRepository;
            _moduleClient = moduleClient;
            _clock = clock;
        }
        public async Task HandleAsync(AddGroupMember command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            var user = await _moduleClient.GetAsync<UserDto>("/identity/me", new MeQuery(command.UserIdOfMember));
            group.AddMember(UserId.From(command.UserIdOfMember), MemberName.Create(user.Nickname),_clock);
        }
    }
}