using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Group.Create.ExternalQueries;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Modules;
using NodaTime;

namespace InstantMessenger.Groups.Api.Features.Group.Create
{
    internal sealed class CreateGroupHandler : ICommandHandler<CreateGroupCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModuleClient _client;
        private readonly IGroupRepository _groupRepository;
        private readonly IClock _clock;

        public CreateGroupHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork, IModuleClient client, IClock clock)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
            _client = client;
            _clock = clock;
        }

        public async Task HandleAsync(CreateGroupCommand command)
        {
            var me = await _client.GetAsync<UserDto>("/identity/me", new MeQuery(command.UserId));
            var group = Domain.Entities.Group.Create(GroupId.From(command.GroupId), GroupName.Create(command.GroupName), UserId.From(command.UserId), MemberName.Create(me.Nickname), _clock);

            await _groupRepository.AddAsync(group);
            await _unitOfWork.Commit();
        }
    }
}