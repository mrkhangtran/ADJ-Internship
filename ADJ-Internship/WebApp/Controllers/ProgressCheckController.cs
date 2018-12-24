﻿using System;
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
    public async Task<ActionResult> Index(int? pageIndex, string PONumberSearch = null, string ItemSearch = null, string Suppliers = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null,string Status=null)
    {
      string check = PONumberSearch + ItemSearch + Suppliers + Factories + Origins + OriginPorts + Depts+Status;
      GetItemSearchDto getSearchItem = await _prcService.SearchItem();
      ViewBag.Suppliers = getSearchItem.Suppliers;
      ViewBag.Origins = getSearchItem.Origins;
      ViewBag.OriginPorts = getSearchItem.OriginPorts;
      ViewBag.Factories = getSearchItem.Factories;
      ViewBag.Depts = getSearchItem.Depts;
      ViewBag.Status = getSearchItem.Status;
      int current = pageIndex ?? 1;
      ViewBag.pageIndex = current;
      PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync(current, 2, PONumberSearch, ItemSearch, Suppliers, Factories, Origins, OriginPorts, Depts,Status);
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
      bool checkedItem = false;
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
            checkedItem = true;
          }        
        }
      }
      if(checkedItem==false||!ModelState.IsValid)
      {
        ViewBag.Check = 2;
      }
      if (progressCheckDTOs.Items == null)
      {
        ViewBag.Check = 3;
      }
      
      GetItemSearchDto getSearchItem = await _prcService.SearchItem();
      ViewBag.Suppliers = getSearchItem.Suppliers;
      ViewBag.Origins = getSearchItem.Origins;
      ViewBag.OriginPorts = getSearchItem.OriginPorts;
      ViewBag.Factories = getSearchItem.Factories;
      ViewBag.Depts = getSearchItem.Depts;
      ViewBag.Status = getSearchItem.Status;
      ViewBag.POUpdate = POUpdate;
      PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync();
      return View("Index",lstPrc);
    }
  }
}