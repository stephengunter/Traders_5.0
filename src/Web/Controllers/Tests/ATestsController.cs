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
using ApplicationCore.Auth.ApiKey;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore;

namespace Web.Controllers.Tests
{
    [Authorize(Policy = "ApiKey_Admin")]
    public class ATestsController : BaseTestController
    {
        private readonly AppSettings _appSettings;
        public ATestsController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string userId = CurrentUserId;
            string name = CurrentUserName;
            var roles = CurrentUseRoles;
            bool isSubscriber = CurrentUserIsSubscriber;
           
            return Ok();
        }


        [HttpGet("version")]
        public ActionResult Version()
        {
            return Ok(_appSettings.ApiVersion);
        }


        [HttpGet("ex")]
        public ActionResult Ex()
        {
            throw new System.Exception("Test Throw Exception");
        }
    }
}
