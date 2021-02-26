using System;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Events;
using InstantMessenger.Identity.Domain.Exceptions;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;
using InstantMessenger.SharedKernel;
using Microsoft.AspNetCore.Identity;

namespace InstantMessenger.Identity.Domain.Entities
{
    public class UserId : EntityId
    {
        public UserId(Guid value) : base(value)
        {
        }

        public static UserId Create() => new(Guid.NewGuid());
    }

    public sealed class User : Entity<UserId>
    {
        public Email Email { get; }
        public string PasswordHash { get; private set; }

        public bool IsVerified { get; private set; }

        public Nickname Nickname { get; private set; }
        public Avatar Avatar { get; private set; } = null;
        private User(){}
        private User(UserId id, Email email, string passwordHash, bool isVerified)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            IsVerified = isVerified;
            Nickname = null;
            Apply(new UserCreatedDomainEvent(Id, email, passwordHash));
        }

        public static async Task<User> Create(Email email, Password password, IUniqueEmailRule rule, IPasswordHasher<User> hasher)
        {
            if(!await rule.IsMet(email))
                throw new EmailNotUniqueException(email);

            var hash = hasher.HashPassword(null, password.Value);
            return new User(UserId.Create(), email, hash, false);
        }

        public async Task Activate(ActivationLink link, string token, Nickname nickname, IUniqueNicknameRule rule)
        {
            if (IsVerified)
            {
                throw new InvalidVerificationTokenException();
            }
            if (!link.Verify(token))
            {
                throw new InvalidVerificationTokenException();
            }

            if (!await rule.IsMeet(nickname))
            {
                throw new NicknameInUseException(nickname);
            }
            IsVerified = true;
            Nickname = nickname;
            Apply(new AccountActivatedDomainEvent(Id, Email, nickname));
        }

        public void Change(Password password,IPasswordHasher<User> hasher)
        {
            var hash = hasher.HashPassword(null, password.Value);
            PasswordHash = hash;
        }

        public async Task Change(Nickname nickname, IUniqueNicknameRule rule)
        {
            if (!await rule.IsMeet(nickname))
            {
                throw new NicknameInUseException(nickname);
            }

            Nickname = nickname;
        }

        public void Change(Avatar avatar)
        {
            Avatar = avatar;
        }
    }
}