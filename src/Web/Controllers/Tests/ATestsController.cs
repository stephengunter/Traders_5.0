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
using System;
using ApplicationCore.DataAccess;
using System.Collections.Generic;
using System.Reflection;
using ApplicationCore.Models;
using ApplicationCore.OrderMaker.ViewServices;

namespace Web.Controllers.Tests
{
    public class ATestsController : BaseTestController
    {
        private readonly AppSettings _appSettings;
        private readonly AdminSettings _adminSettings;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        private readonly HistoryContext _context;
        public ATestsController(IOptions<AppSettings> appSettings, IOptions<AdminSettings> adminSettings,
            IUsersService usersService, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _adminSettings = adminSettings.Value;
            _usersService = usersService;
            _mapper = mapper;
        }



        [HttpGet]
        public ActionResult Index()
        {
            var accountSettings = new ApplicationCore.OrderMaker.Models.AccountSettings
            {
                 Account = "9887654"
            };
            var tradeSettings = new ApplicationCore.OrderMaker.Models.TradeSettings
            {
                FileName = "testFileName"    
            };

            var view = accountSettings.MapViewModel(tradeSettings, _mapper);
            return Ok(view);
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
