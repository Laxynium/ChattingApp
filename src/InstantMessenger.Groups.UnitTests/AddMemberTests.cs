using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Group.Create;
using InstantMessenger.Groups.Application.Features.Members.Add;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Groups.UnitTests.Common;
using InstantMessenger.Shared.Modules;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InstantMessenger.Groups.UnitTests
{
    public class AddMemberTests : GroupsModuleUnitTestBase
    {
        private readonly Dictionary<Guid,string> _userIdToNickname = new();
        public AddMemberTests() => Configure(
            sc => sc
                .Remove<IModuleClient>()
                .AddSingleton<IModuleClient>(x => new FakeModuleClient(userIdToNickname:_userIdToNickname))
        );

        
        [Fact]
        public async Task new_member_is_added_with_everyone_role_and_with_his_personal_nickname() => await Run(async sut =>
        {
            var createGroup = new CreateGroupCommand(Guid.NewGuid(), Guid.NewGuid(), "group_name");
            _userIdToNickname.Add(createGroup.UserId,"owner_nickname");
            await sut.SendAsync(createGroup);
            var addMember = new AddGroupMember(createGroup.GroupId, Guid.NewGuid());
            _userIdToNickname.Add(addMember.UserIdOfMember,"member_nickname");

            await sut.SendAsync(addMember);

            var members = await sut.QueryAsync(new GetMembersQuery(createGroup.UserId, addMember.GroupId,userIdOfMember:addMember.UserIdOfMember));
            members.Should().SatisfyRespectively(
                x =>
                {
                    x.UserId.Should().Be(addMember.UserIdOfMember);
                    x.MemberId.Should().NotBeEmpty();
                    x.IsOwner.Should().BeFalse();
                    x.Name.Should().Be("member_nickname");
                    x.CreatedAt.Should().NotBe(default);
                });
            var memberRoles = await sut.QueryAsync(new GetMemberRolesQuery(createGroup.UserId,createGroup.GroupId,addMember.UserIdOfMember));
            memberRoles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );
        });
    }
}