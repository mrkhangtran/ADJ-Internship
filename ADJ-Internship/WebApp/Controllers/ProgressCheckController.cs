using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class ProgressCheckController : Controller
	{
		private readonly IProgressCheckService _prcService;
		public ProgressCheckController(IProgressCheckService prcService)
		{
			_prcService = prcService;
		}
		// GET: ProgressCheckDto
		public async Task<ActionResult> Index(string PONumberSearch = null, string ItemSearch = null, string Suppliers = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null)
		{
			GetItemSearchDto getSearchItem = await _prcService.SearchItem();
			ViewBag.Suppliers = getSearchItem.Suppliers;
			ViewBag.Origins = getSearchItem.Origins;
			ViewBag.OriginPorts = getSearchItem.OriginPorts;
			ViewBag.Factories = getSearchItem.Factories;
			ViewBag.Depts = getSearchItem.Depts;
			ViewBag.ErrorList = "No result match, please try again";
			PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync();
			List<ProgressCheckDto> progressCheckDtos = lstPrc.Items;
			if (PONumberSearch != null)
			{
				progressCheckDtos = progressCheckDtos.Where(p => p.PONumber == PONumberSearch).ToList();
			}
			if (!String.IsNullOrEmpty(Suppliers) || !String.IsNullOrEmpty(Factories))
			{
				progressCheckDtos = progressCheckDtos.Where(p => p.Supplier == Suppliers || p.Factory == Factories).ToList();
			}
			if (!String.IsNullOrEmpty(Origins) || !String.IsNullOrEmpty(OriginPorts))
			{
				progressCheckDtos = progressCheckDtos.Where(p => p.Origin == Origins || p.OriginPort == OriginPorts).ToList();
			}
			List<ProgressCheckDto> model = progressCheckDtos;
			return View("Index", model);
		}
		[HttpPost]
		public async Task<ActionResult> Index(List<ProgressCheckDto> progressCheckDTOs)
		{
			if (ModelState.IsValid)
			{
				for (int i = 0; i < progressCheckDTOs.Count(); i++)
				{
					await _prcService.CreateOrUpdatePurchaseOrderAsync(progressCheckDTOs[i]);
				}
			}
			GetItemSearchDto getSearchItem = await _prcService.SearchItem();
			ViewBag.Suppliers = getSearchItem.Suppliers;
			ViewBag.Origins = getSearchItem.Origins;
			ViewBag.OriginPorts = getSearchItem.OriginPorts;
			ViewBag.Factories = getSearchItem.Factories;
			ViewBag.Depts = getSearchItem.Depts;
			ViewBag.ErrorList = "No result match, please try again";
			PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync();
			List<ProgressCheckDto> progressCheckDtos = lstPrc.Items;
			return View("Index", progressCheckDtos);
		}
	}
}