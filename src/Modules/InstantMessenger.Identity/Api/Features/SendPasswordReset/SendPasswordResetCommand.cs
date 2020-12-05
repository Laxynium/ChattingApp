using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Identity.Api.Features.SendPasswordReset
{
    public class SendPasswordResetCommand : ICommand
    {
        public string Email { get; }

        public SendPasswordResetCommand(string email)
        {
            Email = email;
        }
    }
}