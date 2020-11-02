using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Identity.Domain.Entities
{
    public class ActivationLink : Entity<Guid>
    {
        public string Token { get; }
        
        public Guid UserId { get; }
        
        public DateTimeOffset CreatedAt { get; }

        private ActivationLink(Guid id, string token, Guid userId, DateTimeOffset createdAt) :base(id)
        {
            Token = token;
            UserId = userId;
            CreatedAt = createdAt;
        }

        public static ActivationLink Create(Guid userId, string token)
        {
            return new ActivationLink(Guid.NewGuid(), token, userId, DateTimeOffset.UtcNow);
        }

        public bool Verify(string token) 
            => string.Equals(token, Token, StringComparison.InvariantCultureIgnoreCase);
    }
}