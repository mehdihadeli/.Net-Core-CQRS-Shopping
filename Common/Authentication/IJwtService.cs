using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Common.Authentication
{
    public interface IJwtService
    {
        JsonWebToken GenerateToken(string userId, string userName, IReadOnlyList<string> roles = null,
            IDictionary<string, string> claims = null);

        JsonWebTokenPayload ValidateAndGetTokenPayload(string token,
            TokenValidationParameters tokenValidationParameters = null);

        ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters = null);
    }
}