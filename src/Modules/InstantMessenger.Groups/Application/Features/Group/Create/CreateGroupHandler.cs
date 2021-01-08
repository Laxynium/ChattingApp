using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Features.Group.Create.ExternalQueries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Modules;
using NodaTime;

namespace InstantMessenger.Groups.Application.Features.Group.Create
{
    internal sealed class CreateGroupHandler : ICommandHandler<CreateGroupCommand>
    {
        private readonly IModuleClient _client;
        private readonly IGroupRepository _groupRepository;
        private readonly IClock _clock;

        public CreateGroupHandler(IGroupRepository groupRepository, IModuleClient client, IClock clock)
        {
            _groupRepository = groupRepository;
            _client = client;
            _clock = clock;
        }

        public async Task HandleAsync(CreateGroupCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            if (await _groupRepository.ExistsAsync(groupId))
                throw new GroupAlreadyExistsException(groupId);

            var me = await _client.GetAsync<UserDto>("/identity/me", new MeQuery(command.UserId));
            var group = Domain.Entities.Group.Create(groupId, GroupName.Create(command.GroupName), UserId.From(command.UserId), MemberName.Create(me.Nickname), _clock);

            await _groupRepository.AddAsync(group);
        }
    }
}