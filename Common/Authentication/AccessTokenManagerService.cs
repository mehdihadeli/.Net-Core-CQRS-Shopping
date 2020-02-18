using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Common.Authentication
{
    public class AccessTokenManagerService : IAccessTokenManagerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _redisCache;
        private readonly JwtOptions _jwtOptions;

        public AccessTokenManagerService(IHttpContextAccessor httpContextAccessor, IDistributedCache redisCache,
            JwtOptions jwtOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _redisCache = redisCache;
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> CurrentAccessTokenIsActive()
            => await CurrentAccessTokenIsActive(await GetCurrentAccessTokenAsync());

        public async Task DeactivateCurrentAccessTokenAsync()
            => await DeactivateAccessTokenAsync(await GetCurrentAccessTokenAsync());

        //If there is any value for this token is cache this token revoked
        public async Task<bool> CurrentAccessTokenIsActive(string token)
            => string.IsNullOrWhiteSpace(await _redisCache.GetStringAsync(GetKey(token)));

        //We only store deactivated access token in centralized redis
        public async Task DeactivateAccessTokenAsync(string token)
        {
            await _redisCache.SetStringAsync(GetKey(token), "deactivated", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _jwtOptions.ValidFor
            });
        }

        private async Task<string> GetCurrentAccessTokenAsync()
        {
            //tokens will only be saved to the context if SaveTokens = true; in the startup else will be null although
            //we can get access token from header directly with *_httpContextAccessor.HttpContext.Request.Headers["Authorization"]*
            return await _httpContextAccessor.HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme,
                "access_token");
        }

        private static string GetKey(string token) => $"tokens:{token}";
    }
}