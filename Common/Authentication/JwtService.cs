using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Common.Authentication
{
    public class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;
        private TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger<JwtService> _logger;
        private readonly AuthenticationSettings _settings;

        public JwtService(JwtOptions options, ILogger<JwtService> logger, AuthenticationSettings settings)
        {
            _logger = logger;
            _settings = settings;
            _options = options;
            InitializeJwtParameters();
        }

        #region Public Methods

        public JsonWebToken GenerateToken(string userId, string userName, IReadOnlyList<string> roles = null,
            IDictionary<string, string> claims = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User id claim can not be empty.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("userName claim can not be empty.", nameof(userName));
            }

            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, _options.IssuedAt.ToUnixEpochDate().ToString(),
                    ClaimValueTypes.Integer64)
            };
            if (roles != null && roles.Any())
            {
                var rolesConcatString = string.Join(",", roles);
                jwtClaims.Add(new Claim(ClaimTypes.Role, rolesConcatString));
            }

            var customClaims = claims?.Select(claim => new Claim(claim.Key, claim.Value)).ToArray()
                               ?? Array.Empty<Claim>();

            jwtClaims.AddRange(customClaims);
            if (string.IsNullOrWhiteSpace(_settings.RsaSettings.RsaPrivateKey))
            {
                return null;
            }

            var jwt = new JwtSecurityToken(
                _options.Issuer,
                claims: jwtClaims,
                expires: _options.Expiration,
                notBefore: _options.NotBefore,
                audience: _options.Audience,
                signingCredentials: _options.SigningCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                RefreshToken = string.Empty,
                Expires = _options.Expiration.ToUnixEpochDate(),
                UserId = userId
            };
        }

        public JsonWebTokenPayload ValidateAndGetTokenPayload(string token,
            TokenValidationParameters tokenValidationParameters = null)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal =
                    tokenHandler.ValidateToken(token, tokenValidationParameters ?? _tokenValidationParameters,
                        out var securityToken);
                if (principal == null || !(securityToken is JwtSecurityToken jwt))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var roles = jwt.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

                return new JsonWebTokenPayload
                {
                    UserId = jwt.Subject,
                    UserName = jwt.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Roles = string.IsNullOrWhiteSpace(roles) == false ? roles.Split() : null,
                    Expires = jwt.ValidTo.ToUnixEpochDate(),
                    Claims = jwt.Claims.ToDictionary(k => k.Type, v => v.Value)
                };
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError($"Token validation failed: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters = null)
        {
            try
            {
                var principal =
                    _jwtSecurityTokenHandler.ValidateToken(token,
                        tokenValidationParameters ?? _tokenValidationParameters, out var securityToken);

                if (principal == null || !(securityToken is JwtSecurityToken jwt))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception e)
            {
                _logger.LogError($"Token validation failed: {e.Message}");
                return null;
            }
        }

        #endregion

        private void InitializeJwtParameters()
        {
            _tokenValidationParameters = new TokenValidationParameters
            {
                // Specify the key used to sign the token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _options.SigningCredentials.Key,
                // Ensure the token audience matches our audience value (default true)
                ValidateAudience = _options.ValidateAudience,
                ValidAudience = _options.Audience,
                // Ensure the token was issued by a trusted authorization server (default true)
                ValidateIssuer = _options.ValidateIssuer,
                ValidIssuer = _options.Issuer,
                // Ensure the token hasn't expired
                RequireExpirationTime = true,
                ValidateLifetime = true
            };
        }
    }
}