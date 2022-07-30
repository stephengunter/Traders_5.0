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
using ApplicationCore.Exceptions;

namespace Web.Controllers.User
{
	public class PasswordsController : BaseUserController
	{
		private readonly IUsersService _usersService;
		private const string RULE = "密碼長度不能小於6個字元, 且必須包含數字.";

		public PasswordsController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[HttpGet]
		public async Task<ActionResult> Index()
		{
			var user = await GetCurrentUserAsync(_usersService);

			var model = new SetPasswordRequest { Rule = RULE };
			model.UserHasPassword = await _usersService.HasPasswordAsync(user);

			return Ok(model);
		}

		[HttpPost]
		public async Task<ActionResult> Store([FromBody] SetPasswordRequest model)
		{
			var user = await GetCurrentUserAsync(_usersService);

			bool hasPassword = await _usersService.HasPasswordAsync(user);
			if (hasPassword)
			{
				ModelState.AddModelError("", "密碼設定失敗. 您已經設定過密碼.");
				return BadRequest(ModelState);
			}

			ValidateRequest(model, false);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				await _usersService.AddPasswordAsync(user, model.Password);
			}
			catch (Exception ex)
			{
				if (ex is UserPasswordException)
				{
					ModelState.AddModelError("", RULE);
					return BadRequest(ModelState);
				}
				else
				{
					throw;
				}

			}

			return Ok();
		}

		[HttpPut]
		public async Task<ActionResult> Update([FromBody] SetPasswordRequest model)
		{
			var user = await GetCurrentUserAsync(_usersService);

			bool hasPassword = await _usersService.HasPasswordAsync(user);
			if (!hasPassword)
			{
				ModelState.AddModelError("", "變更密碼失敗. 您從未設定過密碼.");
				return BadRequest(ModelState);
			}

			ValidateRequest(model, true);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				await _usersService.ChangePasswordAsync(user, model.OldPassword, model.Password);
			}
			catch (Exception ex)
			{
				if (ex is WrongPasswordException)
				{
					ModelState.AddModelError("", "變更密碼失敗.");
					return BadRequest(ModelState);
				}
				else if (ex is UserPasswordException)
				{
					ModelState.AddModelError("", RULE);
					return BadRequest(ModelState);
				}
				else
				{
					throw;
				}

			}

			return Ok();
		}


		void ValidateRequest(SetPasswordRequest model, bool checkOldPassword)
		{
			model.Password = model.Password.GetTrimedValue();
			if (model.Password.Length < 6) ModelState.AddModelError("password", "密碼長度不能小於6個字元");

			if (!checkOldPassword) return;

			model.OldPassword = model.OldPassword.GetTrimedValue();
			if (String.IsNullOrEmpty(model.OldPassword)) ModelState.AddModelError("oldPassword", "必須填寫舊密碼");

		}

	}
}
