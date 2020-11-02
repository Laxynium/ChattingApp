using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using InstantMessenger.Identity.Domain.Exceptions;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Identity.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace InstantMessenger.Identity.Domain.Entities
{
    public sealed class User : Entity<Guid>
    {
        public Email Email { get; }
        public string PasswordHash { get; }
        
        public bool IsVerified { get; private set; }

        private User(){}
        private User(Guid id, Email email, string passwordHash, bool isVerified)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            IsVerified = isVerified;
        }

        public static async Task<User> Create(Email email, Password password, IUniqueEmailRule rule, IPasswordHasher<User> hasher)
        {
            if(!await rule.IsMet(email))
                throw new EmailNotUniqueException();

            var hash = hasher.HashPassword(null, password.Value);
            return new User(Guid.NewGuid(), email, hash, false);
        }

        public void Activate(ActivationLink link, string token)
        {
            if (link.Verify(token))
            {
                IsVerified = true;
            }
            else
            {
                throw new InvalidVerificationTokenException();
            }
        }
    }
}