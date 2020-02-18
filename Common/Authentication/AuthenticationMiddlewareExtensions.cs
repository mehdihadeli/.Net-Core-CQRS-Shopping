using Microsoft.AspNetCore.Builder;

namespace Common.Authentication
{
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAccessTokenValidator( this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessTokenMiddleware>();
        }
    }
}