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
    public async Task<ActionResult> Index(int? pageIndex, string PONumberSearch = null, string ItemSearch = null, string Vendor = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null, string Status = null, bool? checkClick = null)
    {
      string check = PONumberSearch + ItemSearch + Vendor + Factories + Origins + OriginPorts + Depts + Status;
      GetItemSearchDto getSearchItem = await _prcService.SearchItem();
      ViewBag.Suppliers = getSearchItem.Suppliers;
      ViewBag.VNPorts = new List<string> { "Cam Pha", "Cua Lo", "Hai Phong", "Hon Gai", "Nghi Son" };
      ViewBag.HKPorts = new List<string> { "Aberdeen", "Crooked Harbour", "Double Haven", "Gin Drinkers Bay", "Inner Port Shelter" };
      ViewBag.Origins = new List<string> { "Hong Kong", "Vietnam" };
      ViewBag.Factories = getSearchItem.Factories;
      ViewBag.Depts = getSearchItem.Depts;
      ViewBag.Status = getSearchItem.Status;
      int current = pageIndex ?? 1;
      ViewBag.pageIndex = current;
      PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync(current, 2, PONumberSearch, ItemSearch, Vendor, Factories, Origins, OriginPorts, Depts, Status);
      lstPrc.CurrentFilter = current.ToString();
      foreach (var item in lstPrc.Items)
      {
        item.ListOrderDetailDto.OrderBy(p => p.ItemNumber);
      }
      if (checkClick == true)
      {
        return PartialView("_SearchingPartial", lstPrc);
      }
      return View("Index", lstPrc);
    }
    [Route("ProgressCheck/Index")]
    [HttpPost]
    public async Task<ActionResult> CreateOrUpdate(PagedListResult<ProgressCheckDto> progressCheckDTOs)
    {
      ViewBag.Check = "empty";
      bool check = false;
      List<string> POUpdate = new List<string>();
      PagedListResult<ProgressCheckDto> lstPrc = new PagedListResult<ProgressCheckDto>();
      lstPrc.Items = new List<ProgressCheckDto>();
      lstPrc.PageCount = progressCheckDTOs.PageCount;
      GetItemSearchDto getSearchItem = await _prcService.SearchItem();
      ViewBag.Suppliers = getSearchItem.Suppliers;
      ViewBag.VNPorts = new List<string> { "Cam Pha", "Cua Lo", "Hai Phong", "Hon Gai", "Nghi Son" };
      ViewBag.HKPorts = new List<string> { "Aberdeen", "Crooked Harbour", "Double Haven", "Gin Drinkers Bay", "Inner Port Shelter" };
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };
      ViewBag.Factories = getSearchItem.Factories;
      ViewBag.Depts = getSearchItem.Depts;
      ViewBag.Status = getSearchItem.Status;
      ViewBag.POUpdate = POUpdate;
      int current = int.Parse(progressCheckDTOs.CurrentFilter);
      ViewBag.pageIndex = current;
      for (int i = 0; i < progressCheckDTOs.Items.Count(); i++)
      {
        for (int j = 0; j < progressCheckDTOs.Items[i].ListOrderDetailDto.Count(); j++)
        {
          if (progressCheckDTOs.Items[i].selected == false)
          {
            string inspectionDate = "Items[" + i + "].InspectionDate";
            string intDate = "Items[" + i + "].IntendedShipDate";
            string id = "Items[" + i + "].Id";
            ModelState[inspectionDate].ValidationState = ModelState[id].ValidationState;
            ModelState[intDate].ValidationState = ModelState[id].ValidationState;
          }        
        }
      }
      if (ModelState.IsValid)
      {
        foreach (var item in progressCheckDTOs.Items)
        {
          if (item.Id == 0 && item.selected == false && item.ListOrderDetailDto.Where(x => x.selected == true).ToList().Count > 0)
          {
            item.selected = true;
          }
          if (item.selected == true || item.ListOrderDetailDto.Where(x => x.selected == true).ToList().Count > 0)
          {
            var temp = await _prcService.CreateOrUpdateProgressCheckAsync(item);
            temp.ShipDate = item.ShipDate;
            lstPrc.Items.Add(temp);
            POUpdate.Add(item.PONumber);
            ViewBag.Check = "success";
            check = true;
          }
          else
          {
            lstPrc.Items.Add(item);
          }
        }
      }
      else
      {
        lstPrc = progressCheckDTOs;
        ViewBag.Check = "invalid";
      }
      return PartialView("_AchievePartial", lstPrc);
    }
  }
}
