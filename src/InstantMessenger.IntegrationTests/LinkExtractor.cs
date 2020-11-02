using System.Linq;
using HtmlAgilityPack;
using MimeKit;

namespace InstantMessenger.IntegrationTests
{
    public static class LinkExtractor
    {
        public static string FromMail(MimeMessage message)
        {
            var body = message.TextBody;
            var doc = new HtmlDocument();
            doc.LoadHtml(body);

            var link = doc.DocumentNode.Descendants("a")
                .Select(x => x.Attributes["href"].Value)
                .FirstOrDefault();
            return link;
        }

    }
}