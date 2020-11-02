namespace InstantMessenger.Shared.MailKit
{
    public class MailOptions
    {
        public string FromEmail { get; }

        public MailOptions(string fromEmail)
        {
            FromEmail = fromEmail;
        }
    }
}