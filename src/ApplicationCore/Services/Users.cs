﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using ApplicationCore.Helpers;
using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Extensions;

namespace ApplicationCore.Services
{
    public interface IUsersService
    {
        Task<User> CreateUserAsync(string email, bool emailConfirmed);

        #region Find
        Task<User> FindUserByApiKeyAsync(string apiKey);
        Task<User> FindUserByEmailAsync(string email);
        Task<User> FindUserByIdAsync(string id);
        #endregion

        #region Roles
        Task<IList<string>> GetRolesAsync(User user);
        Task<IEnumerable<User>> FetchUsersAsync(string role = "");
        IEnumerable<IdentityRole> FetchRoles();
        IEnumerable<IdentityRole> GetRolesByUserId(string userId);
        Task<bool> IsAdminAsync(User user);
        #endregion

        #region ApiKey
        Task<string> SetApiKeyAsync(User user);
        #endregion

        #region Subscriber
        Task<User> FindSubscriberAsync(string userId);
        Task<User> AddSubscriberRoleAsync(string userId);
        Task RemoveSubscriberRoleAsync(string userId);
        #endregion

        #region Password
        Task<bool> HasPasswordAsync(User user);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task AddPasswordAsync(User user, string password);
        Task ChangePasswordAsync(User user, string oldPassword, string password);
        #endregion

    }

    public class UsersService : IUsersService
    {
        DefaultContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersService(DefaultContext context, UserManager<User> userManager, RoleManager<IdentityRole>  roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> CreateUserAsync(string email, bool emailConfirmed)
        {
            var user = new User
            {
                Email = email,
                UserName = email,
                EmailConfirmed = emailConfirmed
            };
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded) return user;

            var error = result.Errors.FirstOrDefault();
            throw new CreateUserException($"{error.Code} : {error.Description}");
        }

        #region Find
        public async Task<User> FindUserByApiKeyAsync(string apiKey) => await _userManager.FindByApiKeyAsync(apiKey);
        public async Task<User> FindUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        public async Task<User> FindUserByIdAsync(string id) => await _userManager.FindByIdAsync(id);
        #endregion

        #region Roles
        string SubscriberRoleName = AppRoles.Subscriber.ToString();
        string DevRoleName = AppRoles.Dev.ToString();
        string BossRoleName = AppRoles.Boss.ToString();

        public async Task<IList<string>> GetRolesAsync(User user) => await _userManager.GetRolesAsync(user);

        public async Task<IEnumerable<User>> FetchUsersAsync(string role = "")
        {
            var users = _userManager.Users;

            if (!String.IsNullOrEmpty(role))
            {
                var selectedRole = await _roleManager.FindByNameAsync(role);
                if (selectedRole != null)
                {
                    var userIdsInRole = _context.UserRoles.Where(x => x.RoleId == selectedRole.Id).Select(b => b.UserId).Distinct().ToList();
                    users = users.Where(user => userIdsInRole.Contains(user.Id));
                }
            }

            return users;
        }

        public IEnumerable<IdentityRole> FetchRoles() => _roleManager.Roles.ToList();

        public IEnumerable<IdentityRole> GetRolesByUserId(string userId)
        {
            var userRoles = _context.UserRoles.Where(x => x.UserId == userId);
            var roleIds = userRoles.Select(ur => ur.RoleId);

            return _roleManager.Roles.Where(r => roleIds.Contains(r.Id));
        }

        public async Task<bool> IsAdminAsync(User user)
        {
            var roles = await GetRolesAsync(user);
            if (roles.IsNullOrEmpty()) return false;

            var match = roles.Where(r => r.Equals(DevRoleName) || r.Equals(BossRoleName)).FirstOrDefault();

            return match != null;
        }
        #endregion

        #region ApiKey

        public async Task<string> SetApiKeyAsync(User user)
        {
            user.ApiKey = Guid.NewGuid().ToString("N");

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new UpdateUserException($"{error.Code} : {error.Description}");
            }

            return user.ApiKey;
        }

        #endregion

        #region Subscriber
        public async Task<User> FindSubscriberAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new UserNotFoundException(userId, "Id");

            bool isSubscriber = await _userManager.IsInRoleAsync(user, SubscriberRoleName);

            return isSubscriber ? user : null;
        }

        public async Task<User> AddSubscriberRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new UserNotFoundException(userId, "Id");

            bool isSubscriber = await _userManager.IsInRoleAsync(user, SubscriberRoleName);
            if (isSubscriber) return user;

            var result = await _userManager.AddToRoleAsync(user, SubscriberRoleName);
            if (result.Succeeded) return user;

            var error = result.Errors.FirstOrDefault();
            throw new AddUserToRoleException($"{error.Code} : {error.Description}");
        }

        public async Task RemoveSubscriberRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new UserNotFoundException(userId, "Id");


            bool isSubscriber = await _userManager.IsInRoleAsync(user, SubscriberRoleName);
            if (isSubscriber)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, SubscriberRoleName);
                if (!result.Succeeded)
                {
                    var error = result.Errors.FirstOrDefault();
                    throw new AddUserToRoleException($"{error.Code} : {error.Description}");
                }
            }

        }
        #endregion

        #region Password
        const string PASSEORD_MISMATCH = "PasswordMismatch";

        public async Task<bool> HasPasswordAsync(User user) => await _userManager.HasPasswordAsync(user);

        public async Task<bool> CheckPasswordAsync(User user, string password) => await _userManager.CheckPasswordAsync(user, password);

        public async Task AddPasswordAsync(User user, string password)
        {
            var result = await _userManager.AddPasswordAsync(user, password);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new UserPasswordException($"{error.Code} : {error.Description}");
            }
        }

        public async Task ChangePasswordAsync(User user, string oldPassword, string password)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, password);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                if (error.Code.EqualTo(PASSEORD_MISMATCH))
                {
                    throw new WrongPasswordException($"{error.Code} : {error.Description}");
                }
                else
                {
                    throw new UserPasswordException($"{error.Code} : {error.Description}");
                }

            }
        }
        #endregion
    }
}
