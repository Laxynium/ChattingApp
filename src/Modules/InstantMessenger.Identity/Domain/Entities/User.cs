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
        public string PasswordHash { get; private set; }

        public bool IsVerified { get; private set; }

        public Nickname Nickname { get; private set; }

        private User(){}
        private User(Guid id, Email email, string passwordHash, bool isVerified)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            IsVerified = isVerified;
            Nickname = null;
        }

        public static async Task<User> Create(Email email, Password password, IUniqueEmailRule rule, IPasswordHasher<User> hasher)
        {
            if(!await rule.IsMet(email))
                throw new EmailNotUniqueException();

            var hash = hasher.HashPassword(null, password.Value);
            return new User(Guid.NewGuid(), email, hash, false);
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
                throw new NicknameInUseException();
            }

            IsVerified = true;
            Nickname = nickname;
        }

        public void ChangePassword(Password password,IPasswordHasher<User> hasher)
        {
            var hash = hasher.HashPassword(null, password.Value);
            PasswordHash = hash;
        }

        public async Task Change(Nickname nickname, IUniqueNicknameRule rule)
        {
            if (!await rule.IsMeet(nickname))
            {
                throw new NicknameInUseException();
            }

            Nickname = nickname;
        }
    }
}