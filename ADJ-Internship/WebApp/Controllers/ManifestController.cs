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
  public class ManifestController : Controller
  {
    private readonly IManifestService _manifestService;
    public ManifestController(IManifestService manifestService)
    {
      _manifestService = manifestService;
    }
    // GET: Manifest
    public async Task<ActionResult> Index(int? pageIndex, string DestinationPort = null, string OriginPort = null, string Carrier = null, DateTime? ETDFrom = null, DateTime? ETDTo = null, string Status = null, string Vendor = null, string PONumber = null, string Item = null, bool? checkClick = null)
    {
      ViewBag.Size = new List<string> { "20GP", "40HC" };
      ViewBag.PackType = new List<string> { "Boxed", "Carton" };
      ViewBag.Loading = new List<string> { "ROAD","SEA","AIR" };
      SearchingManifestItem searchItem = await _manifestService.SearchItem();
      ViewBag.OriginPorts = searchItem.OriginPorts;
      ViewBag.Carriers = searchItem.Carriers;
      ViewBag.Dest = searchItem.DestinationPort;
      ViewBag.Status = searchItem.Status;
      int current = pageIndex ?? 1;
      if (String.IsNullOrEmpty(DestinationPort) && String.IsNullOrEmpty(OriginPort) && String.IsNullOrEmpty(Carrier))
      {
        DestinationPort = searchItem.DestinationPort.FirstOrDefault();
        OriginPort = searchItem.OriginPorts.FirstOrDefault();
        Carrier = searchItem.Carriers.FirstOrDefault();
      }
      ViewBag.pageIndex = current;
      PagedListResult<ShipmentManifestsDtos> listManifest = await _manifestService.ListManifestDtoAsync(current, 2, DestinationPort, OriginPort, Carrier, ETDFrom, ETDTo, Status, Vendor, PONumber, Item);
      if (checkClick == true)
      {
        return PartialView("_SearchingManifestPartial", listManifest);
      }
      return View("Index", listManifest);
    }
    [HttpPost]
    public async Task<ActionResult> CreateOrUpdate(string pageIndex, PagedListResult<ShipmentManifestsDtos> shipmentManifestDtos)
    {
      ViewBag.modalResult = null;
      ViewBag.Size = new List<string> { "20GP", "40HC" };
      ViewBag.PackType = new List<string> { "Boxed", "Carton" };
      ViewBag.Loading = new List<string> { "ROAD","SEA","AIR" };
      SearchingManifestItem searchItem = await _manifestService.SearchItem();
      ViewBag.OriginPorts = searchItem.OriginPorts;
      ViewBag.Carriers = searchItem.Carriers;
      ViewBag.Dest = searchItem.DestinationPort;
      ViewBag.Status = searchItem.Status;
      string DestinationPort = searchItem.DestinationPort.First();
      string OriginPort = searchItem.OriginPorts.First();
      string Carrier = searchItem.Carriers.First();
      PagedListResult<ShipmentManifestsDtos> pagedListResult = new PagedListResult<ShipmentManifestsDtos>();
      for (int i = 0; i < shipmentManifestDtos.Items.Count(); i++)
      {
        for (int j = 0; j < shipmentManifestDtos.Items[i].Manifests.Count(); j++)
        {
          if (shipmentManifestDtos.Items[i].Manifests[j].selectedItem == false && shipmentManifestDtos.Items[i].selectedContainer == true)
          {
            string shipQuantity = "Items[" + i + "].Manifests[" + j + "].ShipQuantity";
            string id = "Items[" + i + "].Manifests[" + j + "].Id";
            ModelState[shipQuantity].ValidationState = ModelState[id].ValidationState;
          }
        }
      }
      foreach (var manifest in shipmentManifestDtos.Items)
      {
        if (_manifestService.checkNameContainer(manifest.Name) == true && manifest.Id == 0)
        {
          ViewBag.nameUnique = "Container name must be unique.";
          pagedListResult = shipmentManifestDtos;
          return PartialView("_AchieveManifestPartial", pagedListResult);
        }
      }
      if (ViewBag.nameUnique == null)
      {
        if (ModelState.IsValid)
        {
          foreach (var manifest in shipmentManifestDtos.Items)
          {
            if (manifest.selectedContainer == true && manifest.Manifests.Where(p => p.selectedItem == true && p.ShipQuantity > 0).Count() > 0)
            {
              await _manifestService.CreateOrUpdateContainerAsync(manifest);
              ViewBag.modalResult = "success";
            }
          }
          pagedListResult = shipmentManifestDtos;
        }
        else
        {
          ViewBag.modalResult = "invalid";
          pagedListResult = shipmentManifestDtos;
        }
      }
      return PartialView("_AchieveManifestPartial", pagedListResult);
    }
  }
}