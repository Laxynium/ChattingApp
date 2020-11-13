using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using MimeKit;

namespace InstantMessenger.IntegrationTests.Common
{
    public static class EmailContentExtractor
    {
        public static string GetUrlFromActivationMail(MimeMessage message)
        {
            var body = message.HtmlBody;
            var doc = new HtmlDocument();
            doc.LoadHtml(body);

            var link = doc.DocumentNode.Descendants("a")
                .Select(x => x.Attributes["href"].Value)
                .FirstOrDefault();
            return link;
        }

        public static string GetTokenFromForgotPasswordEmail(MimeMessage message)
        {
            var body = message.HtmlBody;
            var match = Regex.Match(body, @"Token: \[\[([^\[\]]+)\]\]");
            return match.Groups[1].Value;
        }

        public static (string userId, string token) GetQueryParams(string link)
        {
            var queryParams = HttpUtility.ParseQueryString(new UriBuilder(link).Query);
            return (queryParams.Get("userId"), queryParams.Get("token"));
        }
    }
}