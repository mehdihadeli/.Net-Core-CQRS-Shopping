using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Core.Domain;
using Shopping.Core.Domains;

namespace Shopping.Core.Services
{
    public interface IUserManagerService
    {
        Task<ApplicationUser> CreateUser(ApplicationUser user, string password);
        Task UpdatePassword(int userId, string oldPassword, string newPassword);
        Task<string> GetResetPasswordToken(int userId);
        Task ResetPassword(int userId, string token, string password);
        Task<ApplicationUser> UpdateProfile(int userId, string firstName, string lastName);
        Task<ApplicationUser> GetUserById(int userId);
        Task<ApplicationUser> GetUserByEmailOrUserName(string emailOrUserName);
        Task<bool> CheckForExistingUser(string userNameOrEmail);
        public Task<Result> DeleteUserAsync(int userId);
        public Task<Result> DeleteUserAsync(ApplicationUser user);
        Task<bool> ValidateCredentialsAsync(string usernameOrEmail, string password);
        Task<bool> ValidatePasswordAsync(ApplicationUser user, string password);
        Task<IEnumerable<ApplicationUser>> GetUsers(string firstName, string lastName, string email, int pageindex,
            int pageSize);

        Task<IEnumerable<string>> GetRoles(int userId);
        Task AddRole(int userId, string roleName);
        Task AddRole(int userId, int roleId);
        Task RemoveRole(int userId, string roleName);
        Task RemoveRole(int userId, int roleId);
    }
}