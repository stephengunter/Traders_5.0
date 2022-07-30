using ApplicationCore.Helpers;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ApplicationCore.Auth
{
    public static class ClaimKeys
    {
        public static string Sub = "sub";
        public static string Id = "id";
        public static string Roles = "roles";
        public static string Provider = "provider";
        public static string Name = "name";
        public static string Picture = "picture";
    }
    public static class ClaimsHelper
    {
        public static string GetUserId(this ClaimsPrincipal cp) => cp == null ? "" : cp.Claims.First(c => c.Type.EqualTo(ClaimKeys.Id)).Value;

        public static OAuthProvider GetOAuthProvider(this ClaimsPrincipal cp)
        {
            if (cp == null) return OAuthProvider.Unknown;
            string providerName = cp.Claims.First(c => c.Type.EqualTo(ClaimKeys.Provider)).Value;

            OAuthProvider provider = OAuthProvider.Unknown;
            if (System.Enum.TryParse(providerName, true, out provider))
            {
                if (System.Enum.IsDefined(typeof(OAuthProvider), provider)) return provider;
                else return OAuthProvider.Unknown;
            }
            else return OAuthProvider.Unknown;
        }
    }
}
