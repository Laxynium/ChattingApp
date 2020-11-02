using System;
using System.Security.Cryptography;

namespace InstantMessenger.Identity.Api.Features.SignUp
{
    internal sealed class RandomStringGenerator
    {
        public string Generate(int length)
        {
            var rnd = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(rnd);

            var randomString = Convert.ToBase64String(rnd).Replace("/", "_");
            return randomString;
        }
    }
}