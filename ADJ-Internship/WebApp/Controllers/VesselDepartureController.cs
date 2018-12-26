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
	public class VesselDepartureController : Controller
	{
		private readonly IVesselDepartureService _vesselDepartureService;
		public VesselDepartureController(IVesselDepartureService prcService)
		{
			_vesselDepartureService = prcService;
		}
		// GET: ProgressCheckDto
		public async Task<ActionResult> Index()
		{
			GetItemSearchDto getSearchItem = await _vesselDepartureService.SearchItem();
			ViewBag.Origins = getSearchItem.Origins;
			ViewBag.OriginPorts = getSearchItem.OriginPorts;
			ViewBag.DestPorts = getSearchItem.DestPorts;
			ViewBag.Modes = getSearchItem.Modes;

			return View("Index");
		}
	}
}