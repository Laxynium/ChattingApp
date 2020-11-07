using FluentAssertions;
using FluentAssertions.Collections;
using MimeKit;

namespace InstantMessenger.IntegrationTests.Common
{
    public static class MailMessagesExtensions
    {
        public static AndConstraint<GenericCollectionAssertions<MimeMessage>> HaveSingleMailWithProperReceiverAndSender(this GenericCollectionAssertions<MimeMessage> messages, string from, string to)
            => messages.HaveCount(1).And.SatisfyRespectively(
                m =>
                {
                    m.From.Mailboxes.Should().HaveCount(1).And
                        .SatisfyRespectively(
                            mb => mb.Address.Should().Be(@from)
                        );
                    m.To.Mailboxes.Should().HaveCount(1)
                        .And.SatisfyRespectively(
                            mb => mb.Address.Should().Be(to)
                        );
                }
            );

        public static AndConstraint<GenericCollectionAssertions<MimeMessage>> ContainProperLink(this GenericCollectionAssertions<MimeMessage> messages) 
            => messages.SatisfyRespectively(
                x =>
                {
                    var link = LinkExtractor.FromMail(x);
                    link.Should().NotBeNullOrWhiteSpace();
                }
            );
    }
}