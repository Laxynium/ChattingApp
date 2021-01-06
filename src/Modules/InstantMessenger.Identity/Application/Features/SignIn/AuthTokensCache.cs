using System;
using Microsoft.Extensions.Caching.Memory;

namespace InstantMessenger.Identity.Application.Features.SignIn
{
    internal sealed class AuthTokensCache : IAuthTokensCache
    {
        private readonly IMemoryCache _cache;

        public AuthTokensCache(IMemoryCache cache)
            => _cache = cache;

        public void Set(string email, AuthDto dto)
            => _cache.Set(email, dto, TimeSpan.FromSeconds(30));

        public AuthDto Get(string email)
            => _cache.Get<AuthDto>(email);
    }
}