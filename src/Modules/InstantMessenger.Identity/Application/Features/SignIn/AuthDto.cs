using System;

namespace InstantMessenger.Identity.Application.Features.SignIn
{
    public class AuthDto
    {
        public string Token { get; set; }
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}