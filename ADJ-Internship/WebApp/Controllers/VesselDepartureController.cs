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
		private readonly IVesselDepartureService _vessleDepartureService;
		public VesselDepartureController(IVesselDepartureService vessleDepartureService)
		{
			_vessleDepartureService = vessleDepartureService;
		}
		// GET: Manifest
		public async Task<ActionResult> Index(int? pageIndex, string Origin = null, string OriginPort = null, string Loading = null, DateTime? ETDFrom = null, DateTime? ETDTo = null, string Status = null, string Vendor = null, string PONumber = null, string Item = null,string Warehouse = null, bool? checkClick = null)
		{
			ViewBag.Size = new List<string> { "20GP", "40HC" };
			ViewBag.PackType = new List<string> { "Boxed", "Carton" };
			ViewBag.Loading = new List<string> { "ROAD" };
			SearchItem searchItem = await _vessleDepartureService.SearchItem();
			ViewBag.OriginPorts = searchItem.OriginPorts;
			ViewBag.Carriers = searchItem.Carriers;
			ViewBag.Dest = searchItem.Dest;
			ViewBag.Status = new List<string> { "Pending", "Despatch" };
			ViewBag.Origins = new List<string> { "Vietnam", "HongKong" };
			ViewBag.Voyages = new List<string> { "045FF", "Other" };
			ViewBag.Transhipments = new List<string> { "Confirmred", "Other" };
			ViewBag.Vendors = new List<string> { "Other" };
			int current = pageIndex ?? 1;
			PagedListResult<ContainerDto> listContainer = await _vessleDepartureService.ListContainerAsync(current, 2, Origin, OriginPort, Loading, ETDFrom, ETDTo, Status, Vendor, PONumber, Item, Warehouse);
			if (checkClick == true)
			{
				return PartialView("_SearchingManifestPartial", listContainer);
			}
			return View("Index", listContainer);
		}
		[HttpPost]
		public async Task<ActionResult> CreateOrUpdate(PagedListResult<ShipmentManifestsDtos> shipmentManifestDtos)
		{
			ViewBag.modalResult = null;
			if (ModelState.IsValid)
			{
				foreach (var manifest in shipmentManifestDtos.Items)
				{
					if (manifest.selectedContainer == true)
					{
						await _vessleDepartureService.CreateOrUpdateContainerAsync(manifest);
						ViewBag.modalResult = "success";
					}
				}
			}
			else
			{
				ViewBag.modalResult = "invalid";
			}
			ViewBag.Size = new List<string> { "20GP", "40HC" };
			ViewBag.PackType = new List<string> { "Boxed", "Carton" };
			ViewBag.Loading = new List<string> { "ROAD" };
			SearchItem searchItem = await _vessleDepartureService.SearchItem();
			ViewBag.OriginPorts = searchItem.OriginPorts;
			ViewBag.Carriers = searchItem.Carriers;
			ViewBag.Dest = searchItem.Dest;
			ViewBag.Status = searchItem.Status;
			PagedListResult<ContainerDto> pagedListResult = await _vessleDepartureService.ListContainerAsync();
			return View("Index", pagedListResult);
		}
	}
}