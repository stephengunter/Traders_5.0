using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Web
{
    public static class CustomAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Custom";
    }

    public class CustomAuthOptions : AuthenticationSchemeOptions
    {
        public string UserInfoEndpoint { get; set; }
    }
}
