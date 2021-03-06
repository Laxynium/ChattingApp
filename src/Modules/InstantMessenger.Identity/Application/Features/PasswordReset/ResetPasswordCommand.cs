﻿using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Identity.Application.Features.PasswordReset
{
    public class ResetPasswordCommand : ICommand
    {
        public Guid UserId { get; }
        public string Token { get; }
        public string Password { get; }

        public ResetPasswordCommand(Guid userId, string token, string password)
        {
            UserId = userId;
            Token = token;
            Password = password;
        }
    }
}