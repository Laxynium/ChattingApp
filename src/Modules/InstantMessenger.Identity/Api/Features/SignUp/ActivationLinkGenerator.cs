using System;
using Microsoft.AspNetCore.Http;

namespace InstantMessenger.Identity.Api.Features.SignUp
{
    public class ActivationLinkGenerator
    {
        private readonly IdentityOptions _options;

        public ActivationLinkGenerator(IdentityOptions options)
        {
            _options = options;
        }

        public string Generate(Guid userId, string token)
        {
            var query = new QueryString().Add("userId", userId.ToString())
                .Add("token", token)
                .ToString();
            return new UriBuilder(_options.ClientAppUrlBase)
            {
                Query = query,
                Path = _options.ActivationEndpoint
            }.Uri.ToString();
        }
    }
}