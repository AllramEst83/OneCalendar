using Microsoft.AspNetCore.Identity;
using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Interfaces
{
    public interface IAccountService
    {
        Task<bool> UserExistByUserName(string userEmail);
        Task<bool> RoleExists(string userRole);
        Task<IdentityResult> CreateUser(User userIdentity, string password);
        Task<IdentityResult> CreateRole(string role);
        Task<IdentityResult> AddRoleToUser(User userIdentity, string userRole);
        void SeedRoles();
        Task<IList<string>> GetRolesForUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(string userId);
        Task<IList<string>> GetUserRoles(User user);
        Task<IdentityResult> RemoveRolesFromUser(User user, IList<string> userRoles);
        Task<IdentityResult> DeleteUser(User userIdentity);
        Task<IdentityResult> AddRolesToUser(User userIdentity, IEnumerable<string> userRoles);
    }
}
