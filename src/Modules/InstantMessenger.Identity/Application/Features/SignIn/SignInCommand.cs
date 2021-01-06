using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Identity.Application.Features.SignIn
{
    public class SignInCommand : ICommand
    {
        public string Email { get; }
        public string Password { get; }

        public SignInCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}