using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shopping.Core.Domains;
using Shopping.Core.Services;

namespace Shopping.Application.User.Services
{
    public class RoleManagerService: IRoleManagerService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task AddClaim(string roleName, RoleClaim claim)
        {
            await _roleManager.AddClaimAsync(await GetRole(roleName), new Claim(claim.ClaimType, claim.ClaimValue));
        }

        public async Task AddRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
        }

        public async Task<IEnumerable<RoleClaim>> GetClaims(string roleName)
        {
            return (await _roleManager.GetClaimsAsync(await GetRole(roleName))).Select(c=>new RoleClaim() { ClaimType = c.ValueType, ClaimValue = c.Value });
        }

        public async Task<IdentityRole> GetRole(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<IdentityRole> GetRole(Guid roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }

        public async Task RemoveClaim(string roleName, RoleClaim claim)
        {
            await _roleManager.RemoveClaimAsync(await GetRole(roleName), new Claim(claim.ClaimType, claim.ClaimValue));
        }

        public async Task RemoveRole(string name)
        {
            await _roleManager.DeleteAsync(await GetRole(name));
        }

        public async Task RemoveRole(Guid roleId)
        {
            await _roleManager.DeleteAsync(await GetRole(roleId));
        }
    }
}
