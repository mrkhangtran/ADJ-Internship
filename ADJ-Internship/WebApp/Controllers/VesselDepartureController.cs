using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ADJ.BusinessService.Dtos.VesselDepartureDtos;

namespace WebApp.Controllers
{
	public class VesselDepartureController : Controller
	{
		private readonly IVesselDepartureService _vesselDepartureService;
		public VesselDepartureController(IVesselDepartureService vesselDepartureService)
		{
			_vesselDepartureService = vesselDepartureService;
		}


		public async Task<ActionResult> Index(int? pageIndex,OrderStatus Status, DateTime ETDFroms, DateTime ETDTos, string PONumberSearch = null,
			string ItemSearch = null, string Origins = null, string OriginPorts = null, bool? checkClick = null)
		{
			GetItemSearchDto getSearchItem = await _vesselDepartureService.SearchItem();
			ViewBag.Origins = getSearchItem.Origins;
			ViewBag.OriginPorts = getSearchItem.OriginPorts;
			ViewBag.DestPorts = getSearchItem.DestPorts;
			ViewBag.Modes = getSearchItem.Modes;

			int current = pageIndex ?? 1;
			int pageSize = 2;
			ViewBag.pageIndex = current;
			PagedListResult<ContainerDto> pageListResult = await _vesselDepartureService.pagedListContainerAsync(Status, ETDFroms, ETDTos,current,pageSize, PONumberSearch, ItemSearch, Origins, OriginPorts);
			if (checkClick == true)
			{
				return PartialView("_SearchingPartial", pageListResult);
			}
			return View("Index",pageListResult);
		}
	}
}


//public async Task<ActionResult> Index(int? pageIndex, string PONumberSearch = null, string ItemSearch = null,
//			string Origins = null, string OriginPorts = null, OrderStatus Status = null)
//{
//	string check = PONumberSearch + ItemSearch + Suppliers + Factories + Origins + OriginPorts + Depts + Status;
//	GetItemSearchDto getSearchItem = await _prcService.SearchItem();
//	ViewBag.Suppliers = getSearchItem.Suppliers;
//	ViewBag.Origins = getSearchItem.Origins;
//	ViewBag.OriginPorts = getSearchItem.OriginPorts;
//	ViewBag.Factories = getSearchItem.Factories;
//	ViewBag.Depts = getSearchItem.Depts;
//	ViewBag.Status = getSearchItem.Status;
//	int current = pageIndex ?? 1;
//	ViewBag.pageIndex = current;
//	PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync(current, 2, PONumberSearch, ItemSearch, Suppliers, Factories, Origins, OriginPorts, Depts, Status);
//	if (checkClick == true)
//	{
//		return PartialView("_SearchingPartial", lstPrc);
//	}
//	return View("Index", lstPrc);
//}



//public async Task<ActionResult> Index(OrderStatus Status, DateTime ETDFroms, DateTime ETDTos,
//	int pageIndex = 1, int pageSize = 2, string PONumberSearch = null,
//	string ItemSearch = null, string Origins = null, string OriginPorts = null)