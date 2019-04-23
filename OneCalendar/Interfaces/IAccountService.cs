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
    }
}
