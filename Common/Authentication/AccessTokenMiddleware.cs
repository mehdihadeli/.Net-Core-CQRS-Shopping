using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Authentication
{
    public class AccessTokenMiddleware : IMiddleware
    {
        private readonly IAccessTokenManagerService _accessTokenManagerService;

        public AccessTokenMiddleware(IAccessTokenManagerService accessTokenManagerService)
        {
            _accessTokenManagerService = accessTokenManagerService;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _accessTokenManagerService.CurrentAccessTokenIsActive())
            {
                await next(context);
                
                return;
            }
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        }
    }
}