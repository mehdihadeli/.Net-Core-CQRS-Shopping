using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Shopping.Core.Domains;

namespace Shopping.Application.Services
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityClaimsProfileService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim("userid", user.Id.ToString()),
                new Claim("email", user.Email),
            };

            //Add roles and claims
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                claims = claims.Concat(roles.Select(r => new Claim("role", r)));
                foreach (var roleName in roles)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        claims = claims.Concat(roleClaims);
                    }
                }
            }

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}