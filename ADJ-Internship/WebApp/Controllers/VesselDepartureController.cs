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
		private readonly int pageSize;

		public VesselDepartureController(IVesselDepartureService vesselDepartureService)
		{
			_vesselDepartureService = vesselDepartureService;
			pageSize = 3;
		}

		public async Task<ActionResult> Index()
		{
			await SetDropDownListAsync();

			VesselDepartureDtos result = await _vesselDepartureService.ListContainerDtoAsync(null, null, null, null, null, null, null, null);

			return View("Index", result);
		}

		[HttpPost]
		public async Task<ActionResult> Filter(VesselDepartureDtos filter, int? pageIndex)
		{
			await SetDropDownListAsync();

			VesselDepartureDtos result = await _vesselDepartureService.ListContainerDtoAsync(filter.filterDto.containerId, filter.filterDto.origin, filter.filterDto.originPort, filter.filterDto.status, pageIndex, pageSize, filter.filterDto.etdFrom, filter.filterDto.etdTo);

			return View("Index", result);
		}
		
		public async Task SetDropDownListAsync()
		{
			ViewBag.Origin = new List<string> { "VietNam", "HongKong" }.OrderBy(x => x).ToList();
			ViewBag.Mode = new List<string> { "Road", "Sea","Air" }.OrderBy(x => x).ToList();
			ViewBag.Status = new List<string> { "Pending", "Despatch" }.OrderBy(x => x).ToList();

			SearchItem searchItem = await _vesselDepartureService.SearchItem();
			ViewBag.OriginPort = searchItem.OriginPorts.OrderBy(x => x).ToList();
			ViewBag.Carrier = searchItem.Carriers.OrderBy(x => x).ToList();
			ViewBag.Dest = searchItem.DestPorts.OrderBy(x => x).ToList();


			ViewBag.PageSize = pageSize;
			ViewBag.PageIndex = 1;
		}

		[HttpPost]
		public async Task<ActionResult> Achive(VesselDepartureDtos model)
		{
			await SetDropDownListAsync();

			VesselDepartureDtos result =  _vesselDepartureService.Achive(model);

			return View("Index",result);
		}
	}
}