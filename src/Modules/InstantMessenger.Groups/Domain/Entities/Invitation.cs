using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Events;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Rules;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;
using NodaTime;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class Invitation : Entity<InvitationId>
    {

        public GroupId GroupId { get; }

        public InvitationCode InvitationCode { get; }

        private ExpirationTimeContainer _expirationTime = new ExpirationTimeContainer();
        public ExpirationTime ExpirationTime 
        { 
            get => _expirationTime.ExpirationTime; 
            set => _expirationTime.ExpirationTime = value;
        }

        private UsageCounterContainer _usageCounter = new UsageCounterContainer();
        public UsageCounter UsageCounter 
        { 
            get => _usageCounter.UsageCounter;
            set => _usageCounter.UsageCounter = value;
        }

        private Invitation(){}

        private Invitation(InvitationId invitationId, GroupId groupId, InvitationCode invitationCode, ExpirationTime expirationTime, UsageCounter usageCounter) : base(invitationId)
        {
            GroupId = groupId;
            InvitationCode = invitationCode;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
            Apply(new InvitationCreatedEvent(invitationId, groupId, invitationCode, expirationTime, usageCounter));
        }

        public static async Task<Invitation> Create(InvitationId invitationId, GroupId groupId,
            ExpirationTime expirationTime, UsageCounter usageCounter, IUniqueInvitationCodeRule uniqueInvitationCodeRule)
        {
            var code = await CreateInvitationCode(uniqueInvitationCodeRule);
            var invitation = new Invitation(invitationId, groupId, code, expirationTime, usageCounter);
            return invitation;
        }

        public void Use(UserId userId, Group group, IClock clock)
        {
            if(group.ContainsMember(userId))
                throw new InvalidInvitationException();
            if (ExpirationTime.IsExpired(clock.GetCurrentInstant().InUtc().ToDateTimeOffset()))
                throw new InvalidInvitationException();
            if(!UsageCounter.CanUse())
                throw new InvalidInvitationException();

            UsageCounter = UsageCounter.Use();
            Apply(new InvitationUsedDomainEvent(userId, this.Id, group.Id));
        }

        public void Revoke(UserId userId)
        {
            Apply(new InvitationRevokedEvent(Id, GroupId,InvitationCode, ExpirationTime, UsageCounter));
        }

        private static async Task<InvitationCode> CreateInvitationCode(IUniqueInvitationCodeRule uniqueInvitationCodeRule)
        {
            InvitationCode code;
            do
            {
                code = InvitationCode.Create(new RandomStringGenerator());
            } while (!await uniqueInvitationCodeRule.IsMeet(code));

            return code;
        }
    }

    public sealed class RandomStringGenerator
    {
        public string Generate(int length, string charSet)
        {
            using var rng = new RNGCryptoServiceProvider();
            return Enumerable.Range(0, length).Select(_ => charSet[GetInt(rng, charSet.Length)])
                .Aggregate(new StringBuilder(),
                    (acc,x)=>acc.Append(x),
                    x=>x.ToString());
        }

        private static int GetInt(RNGCryptoServiceProvider rnd, int max)
        {
            var r = new byte[4];
            int value;
            do
            {
                rnd.GetBytes(r);
                value = BitConverter.ToInt32(r, 0) & int.MaxValue;
            } while (value >= max * (int.MaxValue / max));
            return value % max;
        }
    }

}