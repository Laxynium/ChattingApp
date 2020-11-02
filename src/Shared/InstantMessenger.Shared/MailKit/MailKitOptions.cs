namespace InstantMessenger.Shared.MailKit
{
    internal sealed class MailKitOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public bool Authenticate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }
}