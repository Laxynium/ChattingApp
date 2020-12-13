using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using InstantMessenger.Groups.Api.Features.Invitations.GenerateInvitationCode;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Testing;
using Xunit;
using IClock = NodaTime.IClock;

namespace InstantMessenger.UnitTests
{
    public class GenerateInvitationTests : GroupsModuleUnitTestBase
    {
        private readonly DateTimeOffset _date = new DateTimeOffset(new DateTime(2020,1,1));
        private readonly FakeClock _clock;
        public GenerateInvitationTests()
        {
            _clock = new FakeClock(Instant.FromDateTimeOffset(_date));
            Configure(
                x =>
                {
                    x.Remove<IClock>();
                    x.AddSingleton<IClock>(_clock);
                }
            );
        }

        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var command = new GenerateInvitationCommand(Guid.NewGuid(), Guid.NewGuid(),  Guid.NewGuid(),
                new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite),
                new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite));

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_member_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1").Build();
            var command = new GenerateInvitationCommand(Guid.NewGuid(), group.GroupId, Guid.NewGuid(),
                new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite),
                new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite));

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });

        [Fact]
        public async Task Member_without_correct_permissions_cannot_generate_invitation() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateMember().Build()
                .Build().Build();
            var command = new GenerateInvitationCommand(group.Member(1).UserId, group.GroupId, Guid.NewGuid(),
                new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite),
                new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite));

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });


        [Fact]
        public async Task Owner_can_create_invitation() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .Build();
            var command = new GenerateInvitationCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), 
                new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite), 
                new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite));

            await sut.SendAsync(command);

            var invitationDto = await sut.QueryAsync(new GetInvitationQuery(@group.OwnerId, command.GroupId,command.InvitationId));
            invitationDto.Code.Should().HaveLength(8).And.MatchRegex("[a-zA-Z0-9]{8,8}");
            invitationDto.GroupId.Should().Be(group.GroupId);
            invitationDto.InvitationId.Should().Be(command.InvitationId);
            invitationDto.ExpirationTime.Should().BeEquivalentTo(
                new ExpirationTimeDto
                {
                    Type = ExpirationTimeTypeDto.Infinite,
                    Period = null,
                    Start = _date
                }
            );
            invitationDto.UsageCounter.Should().BeEquivalentTo(new UsageCounterDto()
            {
                Value = null,
                Type = UsageCounterTypeDto.Infinite
            });
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageInvitations")]
        public async Task Member_with_correct_permission_can_create_invitation(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(1).Build().Build()
                .Build();
            var command = new GenerateInvitationCommand(group.Member(1).UserId, group.GroupId, Guid.NewGuid(), 
                new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite), 
                new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite));

            await sut.SendAsync(command);

            var invitationDto = await sut.QueryAsync(new GetInvitationQuery(@group.OwnerId, group.GroupId,command.InvitationId));
            invitationDto.Code.Should().HaveLength(8).And.MatchRegex("[a-zA-Z0-9]{8,8}");
            invitationDto.GroupId.Should().Be(group.GroupId);
            invitationDto.InvitationId.Should().Be(command.InvitationId);
            invitationDto.ExpirationTime.Should().BeEquivalentTo(
                new ExpirationTimeDto
                {
                    Type = ExpirationTimeTypeDto.Infinite,
                    Period = null,
                    Start = _date
                }
            );
            invitationDto.UsageCounter.Should().BeEquivalentTo(new UsageCounterDto()
            {
                Value = null,
                Type = UsageCounterTypeDto.Infinite
            });
        });
    }
}