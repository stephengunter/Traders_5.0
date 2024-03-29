﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Views;
using ApplicationCore.Settings;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.Cors;
using ApplicationCore.Authorization;
using System.Threading.Tasks;
using ApplicationCore.Services;
using ApplicationCore.Exceptions;

namespace Web.Controllers
{
	[EnableCors("Global")]
	[Route("[controller]")]
	public abstract class BaseController : ControllerBase
	{
		protected string RemoteIpAddress => Request.HttpContext.Connection.RemoteIpAddress?.ToString();

		protected IActionResult RequestError(string key, string msg)
		{
			ModelState.AddModelError(key, msg);
			return BadRequest(ModelState);
		}

		protected string MailTemplatePath(IWebHostEnvironment environment, AppSettings appSettings)
			=> Path.Combine(environment.WebRootPath, appSettings.TemplatePath.HasValue() ? appSettings.TemplatePath : "templates");


		protected string GetMailTemplate(IWebHostEnvironment environment, AppSettings appSettings, string name = "default")
		{
			var pathToFile = Path.Combine(MailTemplatePath(environment, appSettings), $"{name}.html");
			if (!System.IO.File.Exists(pathToFile)) throw new Exception("email template file not found: " + pathToFile);

			string body = "";
			using (StreamReader reader = System.IO.File.OpenText(pathToFile))
			{
				body = reader.ReadToEnd();
			}

			return body.Replace("APPNAME", appSettings.Title).Replace("APPURL", appSettings.ClientUrl);

		}

		

		#region  CurrentUser
		protected string CurrentUserName => User.Claims.UserName();
		protected string CurrentUserId => User.Claims.UserId();
		protected IEnumerable<string> CurrentUseRoles => User.Claims.Roles();
		protected bool CurrentUserIsSubscriber => User.Claims.IsSubscriber();

		protected async Task<ApplicationCore.Models.User> GetCurrentUserAsync(IUsersService usersService)
		{
			var user = await usersService.FindUserByIdAsync(CurrentUserId);
			if (user == null) throw new UserNotFoundException(CurrentUserId);

			return user;
		}
		#endregion

	}

	[EnableCors("Api")]
	[Route("api/[controller]")]
	public abstract class BaseApiController : BaseController
	{
		
	}

	[EnableCors("Admin")]
	[Route("admin/[controller]")]
	[Authorize(Policy = "Admin")]
	public class BaseAdminController : BaseController
	{
		protected void ValidateRequest(AdminRequest model, AdminSettings adminSettings)
		{
			if (model.Key != adminSettings.Key) ModelState.AddModelError("key", "認證錯誤");

		}
	}

	[EnableCors("Global")]
	[Route("user/[controller]")]
	[Authorize]
	public class BaseUserController : BaseController
	{
		
	}

	[Route("tests/[controller]")]
	public abstract class BaseTestController : BaseController
	{

	}


}
