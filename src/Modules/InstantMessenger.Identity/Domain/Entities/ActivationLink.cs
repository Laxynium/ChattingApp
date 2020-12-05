using System;
using InstantMessenger.Identity.Domain.Events;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Identity.Domain.Entities
{
    public class ActivationLinkId : EntityId
    {
        public ActivationLinkId(Guid value) : base(value)
        {
        }
        public static ActivationLinkId Create()=>new ActivationLinkId(Guid.NewGuid());
    }
    public class ActivationLink : Entity<ActivationLinkId>
    {
        public string Token { get; }
        
        public UserId UserId { get; }
        
        public DateTimeOffset CreatedAt { get; }

        private ActivationLink(){}

        private ActivationLink(ActivationLinkId id, string token, UserId userId, DateTimeOffset createdAt) :base(id)
        {
            Token = token;
            UserId = userId;
            CreatedAt = createdAt;
            Apply(new ActivationLinkCreatedDomainEvent(Id, Token, userId, CreatedAt));
        }

        public static ActivationLink Create(UserId userId, string token)
        {
            return new ActivationLink(ActivationLinkId.Create(), token, userId, DateTimeOffset.UtcNow);
        }

        public bool Verify(string token) 
            => string.Equals(token, Token, StringComparison.InvariantCultureIgnoreCase);
    }
}