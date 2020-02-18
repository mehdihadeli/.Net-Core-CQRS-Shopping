using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Common.Authentication
{
    public class JwtOptions
    {
        /// <summary>
        /// 4.1.1.  "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 4.1.2.  "sub" (Subject) Claim - The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 4.1.3.  "aud" (Audience) Claim - The "aud" (audience) claim identifies the recipients that the JWT is intended for.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 4.1.4.  "exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(ValidFor);

        /// <summary>
        /// 4.1.5.  "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public DateTime NotBefore => DateTime.Now;

        /// <summary>
        /// 4.1.6.  "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the time at which the JWT was issued.
        /// </summary>
        public DateTime IssuedAt => DateTime.Now;

        /// <summary>
        /// Set the timespan the token will be valid for (default is 120 min)
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Set Https on metadata addresses or authority
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// bearer token should be stored in the AuthenticationProperties after a successful authorization.
        /// </summary>
        public bool SaveToken { get; set; }

        /// <summary>
        /// Check for validating audience
        /// </summary>
        public bool ValidateAudience { get; set; }

        /// <summary>
        /// Check for validating issuer
        /// </summary>
        public bool ValidateIssuer { get; set; }

        /// <summary>
        /// check for validating token lifetime
        /// </summary>
        public bool ValidateLifetime { get; set; }

        /// <summary>
        /// check for expiration time
        /// </summary>
        public bool RequireExpirationTime { get; set; }
    }
}