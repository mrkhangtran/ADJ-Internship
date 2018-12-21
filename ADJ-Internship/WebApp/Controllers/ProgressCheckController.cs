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
    public async Task<ActionResult> Index(int? pageIndex, string PONumberSearch = null, string ItemSearch = null, string Suppliers = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null)
    {
      string check = PONumberSearch + ItemSearch + Suppliers + Factories + Origins + OriginPorts + Depts;
      GetItemSearchDto getSearchItem = await _prcService.SearchItem();
      ViewBag.Suppliers = getSearchItem.Suppliers;
      ViewBag.Origins = getSearchItem.Origins;
      ViewBag.OriginPorts = getSearchItem.OriginPorts;
      ViewBag.Factories = getSearchItem.Factories;
      ViewBag.Depts = getSearchItem.Depts;
      int current = pageIndex ?? 1;
      ViewBag.pageIndex = current;
      PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync(current, 2, PONumberSearch, ItemSearch, Suppliers, Factories, Origins, OriginPorts, Depts);
      if (!String.IsNullOrEmpty(check))
      {
        return PartialView("_SearchingPartial", lstPrc);
      }
      return View("Index", lstPrc);
    }
    [Route("ProgressCheck/Index")]
    [HttpPost]
    public async Task<ActionResult> CreateOrUpdate(PagedListResult<ProgressCheckDto> progressCheckDTOs)
    {
      ViewBag.Check = 0;
      List<string> POUpdate = new List<string>();
      if (ModelState.IsValid)
      {
        foreach (var item in progressCheckDTOs.Items)
        {
          if (item.selected == true || item.ListOrderDetailDto.Where(x => x.selected == true).ToList().Count > 0)
          {
            await _prcService.CreateOrUpdateProgressCheckAsync(item);
            POUpdate.Add(item.PONumber);
            ViewBag.Check = 1;
          }        
        }
      }
      GetItemSearchDto getSearchItem = await _prcService.SearchItem();
      ViewBag.Suppliers = getSearchItem.Suppliers;
      ViewBag.Origins = getSearchItem.Origins;
      ViewBag.OriginPorts = getSearchItem.OriginPorts;
      ViewBag.Factories = getSearchItem.Factories;
      ViewBag.Depts = getSearchItem.Depts;
      ViewBag.POUpdate = POUpdate;
      PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync();
      return View("Index",lstPrc);
    }
  }
}