using CaseSolutionsTokenValidationParameters.Models;
using Microsoft.AspNetCore.Identity;
using OneCalendar.Data;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.Services
{
    public class AccountService : IAccountService
    {
        public UserManager<User> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public UserContext Context { get; }

        public AccountService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            UserContext context)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            Context = context;
        }

        public async Task<List<ShortHandUsers>> GetAllUsers()
        {
            List<ShortHandUsers> listOfUsers = null;

            listOfUsers = await Task.FromResult(UserManager.Users.Select(x => new ShortHandUsers() { Id = x.Id, UserName = x.UserName }).ToList());

            return listOfUsers;
        }

        public async Task<IdentityResult> AddRolesToUser(User userIdentity, IEnumerable<string> userRoles)
        {
            IdentityResult identityResult = null;

            identityResult = await UserManager.AddToRolesAsync(userIdentity, userRoles);

            SaveChages();

            return identityResult;
        }

        public async Task<IdentityResult> DeleteUser(User userIdentity)
        {
            IdentityResult removeUserResult = null;

            removeUserResult = await UserManager.DeleteAsync(userIdentity);

            SaveChages();

            return removeUserResult;
        }

        public async Task<IdentityResult> RemoveRolesFromUser(User user, IList<string> userRoles)
        {
            IdentityResult removeRolesFromUserResult = null;

            removeRolesFromUserResult = await UserManager.RemoveFromRolesAsync(user, userRoles);

            SaveChages();

            return removeRolesFromUserResult;
        }

        public async Task<bool> UserExistByUserName(string userEmail)
        {
            User result = null;
            if (!string.IsNullOrEmpty(userEmail))
            {
                result = await UserManager.FindByEmailAsync(userEmail);
            }

            return result == null ? false : true;
        }
        public async Task<bool> UserExistById(string userId)
        {
            User result = null;
            if (!string.IsNullOrEmpty(userId))
            {
                result = await UserManager.FindByIdAsync(userId);
            }

            return result == null ? false : true;
        }


        public async Task<User> GetUserById(string userId)
        {
            User user = null;

            user = await UserManager.FindByIdAsync(userId);

            return user;
        }

        public async Task<IList<string>> GetUserRoles(User user)
        {
            IList<string> userRoles = await UserManager.GetRolesAsync(user);

            return userRoles;
        }

        public async Task<bool> RoleExists(string userRole)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(userRole))
            {
                result = await RoleManager.RoleExistsAsync(userRole);
            }

            return result;
        }

        public async Task<IdentityResult> CreateUser(User userIdentity, string password)
        {
            IdentityResult addUserResult = null;
            if (userIdentity != null && !string.IsNullOrEmpty(password))
            {
                addUserResult = await UserManager.CreateAsync(userIdentity, password);

                SaveChages();
            }

            return addUserResult;
        }

        public async Task<IdentityResult> AddRoleToUser(User userIdentity, string userRole)
        {
            IdentityResult addRoleToUserResult = null;
            if (userIdentity != null && !string.IsNullOrEmpty(userRole))
            {
                addRoleToUserResult = await UserManager.AddToRoleAsync(userIdentity, userRole);
                SaveChages();
            }

            return addRoleToUserResult;
        }

        public async Task<IdentityResult> CreateRole(string role)
        {
            IdentityRole identityRole = null;
            IdentityResult addRoleResult = null;

            if (!string.IsNullOrEmpty(role))
            {
                identityRole = new IdentityRole()
                {
                    Name = role
                };

                addRoleResult = await RoleManager.CreateAsync(identityRole);
                SaveChages();
            }

            return addRoleResult;
        }

        private async void SaveChages()
        {
            await Context.SaveChangesAsync();
        }

        public async void SeedRoles()
        {
            bool result = await RoleManager.RoleExistsAsync(TokenValidationConstants.Roles.AdminAccess);

            if (!result)
            {
                List<IdentityRole> listOfRoles = new List<IdentityRole>()
                {
                    new IdentityRole(){Name = TokenValidationConstants.Roles.AdminAccess},
                    new IdentityRole(){Name = TokenValidationConstants.Roles.EditUserAccess},
                    new IdentityRole(){Name = TokenValidationConstants.Roles.CommonUserAccess},
                };

                foreach (IdentityRole role in listOfRoles)
                {
                    await RoleManager.CreateAsync(role);
                }
                SaveChages();

            }
        }

        public async Task<List<Roles>> GetRoles()
        {

            IQueryable<IdentityRole> query = await Task.FromResult(RoleManager.Roles);
            List<Roles> roles = query.Select(x => new Roles()
            {
                Id = x.Id,
                Role = x.Name

            }).ToList();

            return roles;
        }

        public async Task<IList<string>> GetRolesForUser(User user)
        {
            IList<string> userRoles = null;
            if (user != null)
            {
                userRoles = await UserManager.GetRolesAsync(user);
            }

            return userRoles;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = null;

            if (!String.IsNullOrEmpty(email))
            {
                user = await UserManager.FindByEmailAsync(email);
            }

            return user;
        }
    }
}
