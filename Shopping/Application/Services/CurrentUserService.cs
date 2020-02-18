using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shopping.Core.Services;

namespace Shopping.Application.User.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId) == false)
            {
                UserId = int.Parse(userId);
            }
            IsAuthenticated = UserId > 0;
        }

        public int UserId { get; }

        public bool IsAuthenticated { get; }
    }
}
