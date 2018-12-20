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
    public async Task<ActionResult> Index(int? pageIndex)
    {
      GetItemSearchDto getSearchItem = await _prcService.SearchItem();
      ViewBag.Suppliers = getSearchItem.Suppliers;
      ViewBag.Origins = getSearchItem.Origins;
      ViewBag.OriginPorts = getSearchItem.OriginPorts;
      ViewBag.Factories = getSearchItem.Factories;
      ViewBag.Depts = getSearchItem.Depts;
      ViewBag.POUpdate = "null";
      int current = pageIndex ?? 1;
      PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync(current);
      return View("Index", lstPrc);
    }
    [HttpPost]
    public async Task<ActionResult> CreateOrUpdate(PagedListResult<ProgressCheckDto> progressCheckDTOs)
    {
      ViewBag.Check = 0;
      List<string> POUpdate = new List<string>();
      if (ModelState.IsValid)
      {
        for (int i = 0; i < progressCheckDTOs.Items.Count(); i++)
        {
          if (progressCheckDTOs.Items[i].selected == true)
          {
            await _prcService.CreateOrUpdatePurchaseOrderAsync(progressCheckDTOs.Items[i]);
            POUpdate.Add(progressCheckDTOs.Items[i].PONumber);
          }
        }
        ViewBag.Check = 1;
      }
      GetItemSearchDto getSearchItem = await _prcService.SearchItem();
      ViewBag.Suppliers = getSearchItem.Suppliers;
      ViewBag.Origins = getSearchItem.Origins;
      ViewBag.OriginPorts = getSearchItem.OriginPorts;
      ViewBag.Factories = getSearchItem.Factories;
      ViewBag.Depts = getSearchItem.Depts;
      ViewBag.POUpdate = POUpdate;
      PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync(1,2);
      return View("Index",lstPrc);
    }
    public async Task<ActionResult> SearchItem(int? pageIndex, string PONumberSearch = null, string ItemSearch = null, string Suppliers = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null)
    {
      int current = pageIndex ?? 1;
      PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync(current, 2, PONumberSearch, ItemSearch, Suppliers, Factories, Origins, OriginPorts, Depts);
      return PartialView("_SearchingPartial", lstPrc);
    }
  }
}