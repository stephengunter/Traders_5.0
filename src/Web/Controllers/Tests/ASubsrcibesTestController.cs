using ApplicationCore.Helpers;
using ApplicationCore.Services;
using ApplicationCore.Settings;
using ApplicationCore.Views;
using ApplicationCore.ViewServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Models;

namespace Web.Controllers.Tests
{
    public class ASubsrcibesTestController : BaseTestController
    {
        private readonly AdminSettings _adminSettings;
        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;

        public ASubsrcibesTestController(IOptions<AdminSettings> adminSettings, IUsersService usersService, IAuthService authService)
        {
            _adminSettings = adminSettings.Value;
            _usersService = usersService;
            _authService = authService;
        }

        User _testUser = null;
        User TestUser
        {
            get
            {
                if (_testUser == null) _testUser = _usersService.FindUserByEmailAsync(_adminSettings.Email).Result;
                return _testUser;
            }
        }

		[HttpPost]
		public async Task<ActionResult> Test([FromBody] AdminRequest model)
		{
			if (model.Key != _adminSettings.Key) ModelState.AddModelError("key", "認證錯誤");
			if (string.IsNullOrEmpty(model.Cmd)) ModelState.AddModelError("cmd", "指令錯誤");
			if (!ModelState.IsValid) return BadRequest(ModelState);

			if (model.Cmd.EqualTo("login"))
			{
				var responseView = await LoginAsync(RemoteIpAddress);

				return Ok(responseView);
			}
			else
			{
				ModelState.AddModelError("cmd", "指令錯誤");
				return BadRequest(ModelState);
			}

			//return Ok($"{model.Cmd} - OK");
		}

		async Task<AuthResponse> LoginAsync(string remoteIp)
		{
			var user = TestUser;
			var roles = await _usersService.GetRolesAsync(user);
			var oAuth = _authService.FindOAuthByProvider(user.Id, OAuthProvider.Google);
			return await _authService.CreateTokenAsync(remoteIp, user, oAuth, roles);
		}


	}
}
