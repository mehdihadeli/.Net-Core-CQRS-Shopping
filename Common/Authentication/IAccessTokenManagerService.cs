using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.Authentication
{
    public interface IAccessTokenManagerService
    {
        Task<bool> CurrentAccessTokenIsActive();
        Task DeactivateCurrentAccessTokenAsync();
        Task<bool> CurrentAccessTokenIsActive(string token);
        Task DeactivateAccessTokenAsync(string token);
    }
}