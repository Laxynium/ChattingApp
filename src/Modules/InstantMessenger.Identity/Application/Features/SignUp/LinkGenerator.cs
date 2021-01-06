using System;
using Microsoft.AspNetCore.Http;

namespace InstantMessenger.Identity.Application.Features.SignUp
{
    public class LinkGenerator
    {
        private readonly IdentityOptions _options;

        public LinkGenerator(IdentityOptions options)
        {
            _options = options;
        }

        public string GenerateActivationLink(Guid userId, string token)
        {
            var query = new QueryString()
                .Add("userId", userId.ToString())
                .Add("token", token)
                .ToString();
            return new UriBuilder(_options.ClientAppUrlBase)
            {
                Query = query,
                Path = _options.ActivationEndpoint
            }.Uri.ToString();
        }
        public string GenerateResetPasswordLink(Guid userId, string token)
        {
            var query = new QueryString()
                .Add("userId", userId.ToString())
                .Add("token", token)
                .ToString();
            return new UriBuilder(_options.ClientAppUrlBase)
            {
                Query = query,
                Path = _options.ResetPasswordEndpoint
            }.Uri.ToString();
        }
    }
}