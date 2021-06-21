using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Views;
using ApplicationCore.Helpers;
using AutoMapper;
using ApplicationCore.ViewServices;
using Web.Models;
using Web.Helpers;
using Web.Controllers;

namespace Web.Controllers.Admin
{
	public class HistotiesController : BaseAdminController
	{
		private readonly IUsersService _usersService;
		private readonly IMapper _mapper;

		public HistotiesController(IUsersService usersService, IMapper mapper)
		{
			_usersService = usersService;
			_mapper = mapper;
		}

		[HttpGet("")]
		public async Task<ActionResult> Index(string symbol = "", string date = "")
		{
			if (String.IsNullOrEmpty(symbol)) //初次載入頁面
			{
				
			}

			

			return Ok();
		}

	}
}
