using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Helpers.Extensions
{
    public static class UserHelpers
    {
        public static async Task<User> FindByApiKeyAsync(this UserManager<User> userManager, string apiKey)
            => await userManager.Users.SingleOrDefaultAsync(x => x.ApiKey == apiKey);
    }
}
