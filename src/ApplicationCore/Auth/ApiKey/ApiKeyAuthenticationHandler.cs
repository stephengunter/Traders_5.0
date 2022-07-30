using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ApplicationCore.Models;
using ApplicationCore.Services;

namespace ApplicationCore.Auth.ApiKey
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string ProblemDetailsContentType = "application/problem+json";
        private readonly IUsersService _usersService;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUsersService usersService) : base(options, logger, encoder, clock)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyConstants.HeaderName, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.NoResult();
            }

            string key = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || String.IsNullOrWhiteSpace(key))
            {
                return AuthenticateResult.NoResult();
            }

            var user = await _usersService.FindUserByApiKeyAsync(key);

            if (user != null)
            {
                var roles = await _usersService.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimKeys.Name, ClaimTypes.Name),
                    new Claim(ClaimKeys.Id, user.Id),
                    new Claim(ClaimKeys.Sub, user.UserName),
                    new Claim(ClaimKeys.Roles, roles.JoinToString())
                };

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.NoResult();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new UnauthorizedProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new ForbiddenProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        }
    }
}
