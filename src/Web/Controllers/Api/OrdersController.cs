using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Views;
using ApplicationCore.Helpers;
using AutoMapper;
using ApplicationCore.ViewServices;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers.Api
{
	[Authorize(Policy = "ApiKey")]
	public class OrdersController : BaseApiController
	{
		private readonly IUsersService _usersService;
		private readonly IOrdersService _ordersService;
		private readonly IMapper _mapper;
		public OrdersController(IUsersService usersService, IOrdersService ordersService, IMapper mapper)
		{
			_usersService = usersService;
			_ordersService = ordersService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult> Index()
		{
			

			return Ok($"id: {CurrentUserId}  name: {CurrentUserName}");
		}

	}
}
