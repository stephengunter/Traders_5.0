using ApplicationCore.Exceptions;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.Users
{
    public class ApiKeyController : BaseUserController
    {
        private readonly IUsersService _usersService;

        public ApiKeyController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return Ok("apikey");

        }

        [HttpPost]
        public async Task<ActionResult> Store()
        {
            var user = await GetCurrentUserAsync(_usersService);
            string apiKey = await _usersService.SetApiKeyAsync(user);

            return Ok(apiKey);
        }
    }
}
