using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Shopping.Core.Domains;

namespace Shopping.Core.Services
{
    public interface IIdentityUserValidatorService
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<ApplicationUser> FindByUsernameAsync(string username);

        Task<(ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult authenticationResult);

        Task<ApplicationUser> AutoProvisionUserAsync(string provider, int userId, IEnumerable<Claim> claims);
    }
}