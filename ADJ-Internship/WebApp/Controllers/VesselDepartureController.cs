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
		public VesselDepartureController(IVesselDepartureService vesselDepartureService)
		{
			_vesselDepartureService = vesselDepartureService;
		}

		public async Task<ActionResult> Index(string origin, string originPort, string container, string status, DateTime etdFrom, DateTime etdTo, int? pageIndex)
		{
			ViewBag.Origin = new List<string> { "VietNam", "HongKong" }.OrderBy(x => x).ToList();
			ViewBag.Mode = new List<string> { "ROAD", "Others" }.OrderBy(x => x).ToList();
			ViewBag.Status = new List<string> { "Pending", "Despatch" }.OrderBy(x => x).ToList();

			SearchItem searchItem = await _vesselDepartureService.SearchItem();
			ViewBag.OriginPort = searchItem.OriginPorts.OrderBy(x => x).ToList();
			ViewBag.Carrier = searchItem.Carriers.OrderBy(x => x).ToList();
			ViewBag.Dest = searchItem.DestPorts.OrderBy(x => x).ToList();
			

			int current = pageIndex ?? 1;
			int pageSize = 2;
			ViewBag.pageIndex = current;
			PagedListResult<ContainerDto> lstContainer = await _vesselDepartureService.ListManifestDtoAsync(origin, originPort, container, status, etdFrom, etdTo, current, pageSize);
			
			return View("Index", lstContainer);
		}
		//[HttpPost]
		//public async Task<ActionResult> CreateOrUpdate(PagedListResult<ShipmentManifestsDtos> shipmentManifestDtos)
		//{
		//	ViewBag.modalResult = null;
		//	if (ModelState.IsValid)
		//	{
		//		foreach (var manifest in shipmentManifestDtos.Items)
		//		{
		//			if (manifest.selectedContainer == true || manifest.Manifests.Where(p => p.selectedItem == true).Count() > 0)
		//			{
		//				await _manifestService.CreateOrUpdateContainerAsync(manifest);
		//				ViewBag.modalResult = "success";
		//			}
		//		}
		//	}
		//	else
		//	{
		//		ViewBag.modalResult = "invalid";
		//	}
		//	ViewBag.Size = new List<string> { "20GP", "40HC" };
		//	ViewBag.PackType = new List<string> { "Boxed", "Carton" };
		//	ViewBag.Loading = new List<string> { "ROAD" };
		//	SearchingManifestItem searchItem = await _manifestService.SearchItem();
		//	ViewBag.OriginPorts = searchItem.OriginPorts;
		//	ViewBag.Carriers = searchItem.Carriers;
		//	ViewBag.Dest = searchItem.DestinationPort;
		//	ViewBag.Status = searchItem.Status;
		//	PagedListResult<ShipmentManifestsDtos> pagedListResult = await _manifestService.ListManifestDtoAsync();
		//	return View("Index", pagedListResult);
		//}
	}
}