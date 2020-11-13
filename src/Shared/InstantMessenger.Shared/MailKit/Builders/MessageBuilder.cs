using MimeKit;

namespace InstantMessenger.Shared.MailKit.Builders
{
    internal sealed class MessageBuilder : IMessageBuilder
    {
        private readonly MimeMessage _message;

        private MessageBuilder()
        {
            _message = new MimeMessage();
        }

        public static IMessageBuilder Create()
            => new MessageBuilder();

        IMessageBuilder IMessageBuilder.WithSender(string senderEmail)
        {
            _message.From.Add(MailboxAddress.Parse(senderEmail));
            return this;
        }

        IMessageBuilder IMessageBuilder.WithReceiver(string receiverEmail)
        {
            _message.To.Add(MailboxAddress.Parse(receiverEmail));
            return this;
        }

        IMessageBuilder IMessageBuilder.WithSubject(string subject)
        {
            _message.Subject = subject;
            return this;
        }

        IMessageBuilder IMessageBuilder.WithSubject(string template, params object[] @params)
            => ((IMessageBuilder)this).WithSubject(string.Format(template, @params));

        IMessageBuilder IMessageBuilder.WithBody(string body)
        {
            _message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };
            return this;
        }

        IMessageBuilder IMessageBuilder.WithBody(string template, params object[] @params)
            => ((IMessageBuilder)this).WithBody(string.Format(template, @params));

        MimeMessage IMessageBuilder.Build()
            => _message;
    }
}