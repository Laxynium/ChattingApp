﻿using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Group.Create.ExternalQueries;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Modules;
using NodaTime;

namespace InstantMessenger.Groups.Api.Features.Members.Add
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModuleClient _moduleClient;
        private readonly IClock _clock;

        public AddGroupHandler(IGroupRepository groupRepository, 
            IUnitOfWork unitOfWork,
            IModuleClient moduleClient,
            IClock clock)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
            _moduleClient = moduleClient;
            _clock = clock;
        }
        public async Task HandleAsync(AddGroupMember command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException();

            var user = await _moduleClient.GetAsync<UserDto>("/identity/me", new MeQuery(command.UserIdOfMember));
            group.AddMember(UserId.From(command.UserIdOfMember), MemberName.Create(user.Nickname),_clock);

            await _unitOfWork.Commit();
        }
    }
}