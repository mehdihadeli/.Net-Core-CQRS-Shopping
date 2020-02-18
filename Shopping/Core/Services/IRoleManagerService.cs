using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shopping.Core.Domains;

namespace Shopping.Core.Services
{
    public interface IRoleManagerService
    {
        Task AddRole(IdentityRole role);
        Task<IdentityRole> GetRole(String name);
        Task<IdentityRole> GetRole(Guid roleId);
        Task RemoveRole(String name);
        Task RemoveRole(Guid roleId);
        Task AddClaim(String roleName, RoleClaim claim);
        Task RemoveClaim(String roleName, RoleClaim claim);
        Task<IEnumerable<RoleClaim>> GetClaims(string roleName);
    }
}
