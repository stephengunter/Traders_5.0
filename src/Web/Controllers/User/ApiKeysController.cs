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

namespace Web.Controllers.User
{
	public class ApiKeysController : BaseUserController
	{
		private readonly IUsersService _usersService;
		private readonly IApiKeysService _apiKeysService;
		private readonly IMapper _mapper;
		public ApiKeysController(IUsersService usersService, IApiKeysService apiKeysService, IMapper mapper)
		{
			_usersService = usersService;
			_apiKeysService = apiKeysService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult> Index()
		{
			var user = await _usersService.FindUserByIdAsync(CurrentUserId);
			if (user == null) return NotFound();

			var apiKeys = await _apiKeysService.FetchAsync(user);
			if (apiKeys.IsNullOrEmpty()) return Ok(new List<ApiKeyViewModel>());

			apiKeys = apiKeys.GetOrdered().ToList();

			return Ok(apiKeys.MapViewModelList(_mapper));
		}

		[HttpPost]
		public async Task<ActionResult> Store()
		{
			var user = await _usersService.FindUserByIdAsync(CurrentUserId);
			if (user == null) return NotFound();

			var existingApiKeys = await _apiKeysService.FetchAsync(user);
			if (existingApiKeys.HasItems())
			{
				ModelState.AddModelError("", "ApiKey設定失敗. 您已經設定過ApiKey.");
				return BadRequest(ModelState);
			}

			var validRoles = await _usersService.GetRolesAsync(user);

			string key = CreateKey();
			var apiKey = new ApiKey
			{
				UserId = user.Id,
				Key = key,
				Name = "Default",
				Role = validRoles.JoinToString()

			};

			apiKey = await _apiKeysService.CreateAsync(apiKey);

			return Ok(apiKey.MapViewModel(_mapper));
		}

		string CreateKey()
		{
			ApiKey existEntity = null;
			string key = "";

			do
            {
				key = Guid.NewGuid().ToString("N");
				existEntity = _apiKeysService.Find(key);

			} while (existEntity != null);

			return key;
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var apiKey = await _apiKeysService.GetByIdAsync(id);
			if (apiKey == null) return NotFound();
			if (apiKey.UserId != CurrentUserId) return NotFound();
		
			await _apiKeysService.DeleteAsync(apiKey);

			return Ok();
		}

	}
}
