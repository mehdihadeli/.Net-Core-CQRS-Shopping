using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace Common.Authentication
{
public class CookieJwtFormat : ISecureDataFormat<AuthenticationTicket>
{
    private readonly IJwtService _jwtService;
    private readonly TokenValidationParameters _validationParameters;

    public CookieJwtFormat(IJwtService jwtService, TokenValidationParameters validationParameters)
    {
        _jwtService = jwtService;
        _validationParameters = validationParameters;
    }

    public string Protect(AuthenticationTicket data)
    {
        throw new System.NotImplementedException();
    }

    public string Protect(AuthenticationTicket data, string purpose)
    {
        throw new System.NotImplementedException();
    }

    public AuthenticationTicket Unprotect(string protectedText)
    {
        return Unprotect(protectedText, null);
    }

    public AuthenticationTicket Unprotect(string protectedText, string purpose)
    {
        var principal = _jwtService.ValidateToken(protectedText, _validationParameters);
        if (principal == null)
        {
            return null;
        }

        return new AuthenticationTicket(principal, new AuthenticationProperties(),
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }
}
}