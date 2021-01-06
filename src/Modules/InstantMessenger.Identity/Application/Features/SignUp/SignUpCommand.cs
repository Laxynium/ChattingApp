using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Identity.Application.Features.SignUp
{
    public class SignUpCommand : ICommand
    {
        public string Email { get; }
        public string Password { get; }

        public SignUpCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}