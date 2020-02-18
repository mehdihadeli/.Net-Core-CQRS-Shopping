namespace Common.Authentication
{
    public class AuthenticationSettings
    {
        public bool UseCookie { get; set; }
        public HMacSettings HMacSettings { get; set; }
        public RsaSettings RsaSettings { get; set; }
    }

    public class RsaSettings
    {
        public bool IsIssuer { get; set; }
        public string RsaPrivateKey { get; set; }
        public string RsaPublicKey { get; set; }
    }

    public class HMacSettings
    {
        public string SecretKey { get; set; }
    }
}