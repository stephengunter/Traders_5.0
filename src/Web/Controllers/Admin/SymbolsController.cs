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
using ApplicationCore;

namespace Web.Controllers.Admin
{
	public class SymbolsController : BaseAdminController
	{
		private readonly ISymbolsService _symbolsService;
		private readonly IMapper _mapper;

		public SymbolsController(ISymbolsService symbolsService, IMapper mapper)
		{
			_symbolsService = symbolsService;
			_mapper = mapper;
		}

		[HttpGet("")]
		public async Task<ActionResult> Index(int active = 1)
		{
			var symbols = await _symbolsService.FetchAsync();
			bool isActive = active.ToBoolean();
			symbols = symbols.Where(x => x.Active == isActive);

			if (symbols.IsNullOrEmpty()) return Ok(new List<SymbolViewModel>());

			symbols = symbols.GetOrdered();

			var models = symbols.MapViewModelList(_mapper);

			return Ok(models);
		}

		[HttpGet("create")]
		public ActionResult Create()
		{
			return Ok(new SymbolViewModel() { Active = false, Order = -1 });
		}

		[HttpPost("")]
		public async Task<ActionResult> Store([FromBody] SymbolViewModel model)
		{
			ValidateRequest(model, CRUDConstants.Create);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var notice = model.MapEntity(_mapper, CurrentUserId);
			notice.Order = model.Active ? 0 : -1;

			notice = await _symbolsService.CreateAsync(notice);

			return Ok(notice.MapViewModel(_mapper));
		}

		[HttpGet("edit/{id}")]
		public async Task<ActionResult> Edit(int id)
		{
			var symbol = await _symbolsService.GetByIdAsync(id);
			if (symbol == null) return NotFound();

			var model = symbol.MapViewModel(_mapper);

			return Ok(model);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Update(int id, [FromBody] SymbolViewModel model)
		{
			var existingEntity = await _symbolsService.GetByIdAsync(id);
			if (existingEntity == null) return NotFound();

			ValidateRequest(model, CRUDConstants.Update);
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var symbol = model.MapEntity(_mapper, CurrentUserId);
			symbol.Order = model.Active ? 0 : -1;

			await _symbolsService.UpdateAsync(existingEntity, symbol);

			return Ok(symbol.MapViewModel(_mapper));
		}

		[HttpPost("off")]
		public async Task<ActionResult> Off([FromBody] SymbolViewModel model)
		{
			var symbol = await _symbolsService.GetByIdAsync(model.Id);
			if (symbol == null) return NotFound();

			symbol.Order = -1;
			await _symbolsService.UpdateAsync(symbol);

			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var symbol = await _symbolsService.GetByIdAsync(id);
			if (symbol == null) return NotFound();

			symbol.Order = -1;
			symbol.SetUpdated(CurrentUserId);
			await _symbolsService.RemoveAsync(symbol);

			return Ok();
		}
		void ValidateRequest(SymbolViewModel model, CRUDConstants action)
		{
			if (String.IsNullOrEmpty(model.Title)) ModelState.AddModelError("title", "必須填寫title");

			if (String.IsNullOrEmpty(model.Code)) ModelState.AddModelError("code", "必須填寫code");

			var existEntity = _symbolsService.GetByCode(model.Code);
			if (existEntity != null)
			{
				if (action == CRUDConstants.Create) ModelState.AddModelError("code", "code重複了");
				else if(action == CRUDConstants.Update && model.Id != existEntity.Id) ModelState.AddModelError("code", "code重複了");
			}

		}

	}
}
