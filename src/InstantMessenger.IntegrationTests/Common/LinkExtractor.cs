using System;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using MimeKit;

namespace InstantMessenger.IntegrationTests.Common
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

        public static (string userId, string token) GetQueryParams(string link)
        {
            var queryParams = HttpUtility.ParseQueryString(new UriBuilder(link).Query);
            return (queryParams.Get("userId"), queryParams.Get("token"));
        }

    }
}