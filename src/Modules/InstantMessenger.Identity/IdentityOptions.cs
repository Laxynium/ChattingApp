using System.Text;

namespace InstantMessenger.Identity
{
    public class IdentityOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public int ExpirationDays { get; set; }
        public byte[] Key => Encoding.ASCII.GetBytes(Secret);

        public string ClientAppUrlBase { get; set; }
        public string ActivationEndpoint { get; set; }
    }
}