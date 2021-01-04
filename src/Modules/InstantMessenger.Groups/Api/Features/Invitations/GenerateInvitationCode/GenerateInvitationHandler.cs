using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.Rules;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;
using NodaTime;

namespace InstantMessenger.Groups.Api.Features.Invitations.GenerateInvitationCode
{
    internal sealed class GenerateInvitationHandler : ICommandHandler<GenerateInvitationCommand>

    {
        private readonly IGroupRepository _groupRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUniqueInvitationCodeRule _uniqueInvitationCodeRule;
        private readonly IClock _clock;

        public GenerateInvitationHandler(
            IGroupRepository groupRepository,
            IInvitationRepository invitationRepository,
            IUniqueInvitationCodeRule uniqueInvitationCodeRule,
            IClock clock)
        {
            _groupRepository = groupRepository;
            _invitationRepository = invitationRepository;
            _uniqueInvitationCodeRule = uniqueInvitationCodeRule;
            _clock = clock;
        }
        public async Task HandleAsync(GenerateInvitationCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);


            var expirationTime = GetExpirationTime(command);
            var usageCounter = GetUsageCounter(command);

            var invitation = await group.GenerateInvitation(
                UserId.From(command.UserId),
                InvitationId.From(command.InvitationId),
                expirationTime,
                usageCounter,
                _uniqueInvitationCodeRule
            );

            await _invitationRepository.AddAsync(invitation);
        }

        private ExpirationTime GetExpirationTime(GenerateInvitationCommand command)
        {
            var now = _clock.GetCurrentInstant().ToDateTimeOffset();
            return command.ExpirationTime switch
            {
                { } dto when dto.Type == ExpirationTimeTypeCommandItem.Infinite => new InfiniteExpirationTime(now),
                { } dto when dto.Type == ExpirationTimeTypeCommandItem.Bounded => new BoundedExpirationTime(
                    now,
                    dto.Period ?? throw new ArgumentException()
                ),
                _ => throw new InvalidExpirationTimeException()
            };
        }

        private UsageCounter GetUsageCounter(GenerateInvitationCommand command)
        {
            return command.UsageCounter switch
            {
                { } dto when dto.Type == UsageCounterTypeCommandItem.Infinite => new InfiniteUsageCounter(),
                { } dto when dto.Type == UsageCounterTypeCommandItem.Bounded => new BoundedUsageCounter(dto.Times ?? throw new ArgumentException()),
                _ => throw new InvalidExpirationTimeException()
            };
        }
    }
}