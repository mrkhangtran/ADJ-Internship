using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Implementations;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class VesselDepartureController : Controller
	{
		private readonly IVesselDepartureService _vesselDepartureService;

		public VesselDepartureController(IVesselDepartureService orderService)
		{
			_vesselDepartureService = orderService;
		}


		public async Task<IActionResult> Index()
		{
			//GetItemSearchDto getSearchItem = await _vesselDepartureService.SearchItem();
			//ViewBag.Origins = getSearchItem.Origins;
			//ViewBag.OriginPorts = getSearchItem.OriginPorts;
			//ViewBag.Vendors = getSearchItem.Vendors;
			return View();
		}
	}
}