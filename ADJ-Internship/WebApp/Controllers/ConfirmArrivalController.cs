using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
  public class ConfirmArrivalController : Controller
  {
    private readonly IConfirmArrivalService _CAService;

    public ConfirmArrivalController(IConfirmArrivalService CAService)
    {
      _CAService = CAService;
    }

    public async Task<ActionResult> Index()
    {
      SetDropDownList();
      ConfirmArrivalDtos model = new ConfirmArrivalDtos();
      model.Containers = new PagedListResult<ConfirmArrivalResultDtos>();

      model.Containers = await _CAService.ListContainerFilterAsync(null, null, null, "HongKong", null, null, null, null);

      if (model.Containers.Items.Count == 0)
      {
        ViewBag.ShowModal = "NoResult";
      }

      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Search(ConfirmArrivalDtos model, string page = null)
    {
      SetDropDownList();
      if (page == null) { page = "1"; }
      int pageIndex = int.Parse(page);
      ViewBag.Page = pageIndex;

      model.Containers = new PagedListResult<ConfirmArrivalResultDtos>();
      
      model.Containers = await _CAService.ListContainerFilterAsync(pageIndex, model.FilterDtos.ETAFrom, model.FilterDtos.ETATo, model.FilterDtos.Origin,
        model.FilterDtos.Mode, model.FilterDtos.Vendor, model.FilterDtos.Container, model.FilterDtos.Status);

      if (model.Containers.Items.Count == 0)
      {
        ViewBag.ShowModal = "NoResult";
      }

      return PartialView("_Result", model);
    }

    public async Task<ActionResult> Achieve(ConfirmArrivalDtos model)
    {
      SetDropDownList();

      if (ModelState.IsValid)
      {
        if (model.Containers != null)
        {
          if (SelectAtLeastOne(model.Containers.Items))
          {
            {
              foreach (var item in model.Containers.Items)
              {
                await _CAService.CreateOrUpdateCAAsync(item.Id, model.ListArrivalDate[item.GroupId]);
              }
              ModelState.Clear();
              ViewBag.ShowModal = "Updated";
            }
          }
          else
          {
            ViewBag.ShowModal = "NoItem";
          }
        }
      }

      return PartialView("_Result", model);
    }

    public bool SelectAtLeastOne(List<ConfirmArrivalResultDtos> input)
    {
      foreach (var item in input)
      {
        if (item.Selected)
        {
          return true;
        }
      }

      return false;
    }

    public void SetDropDownList()
    {
      ViewBag.Modes = new List<string> { "Road", "Sea", "Air" };
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };
      ViewBag.Statuses = new List<string> { ContainerStatus.Despatch.ToString(), ContainerStatus.Arrived.ToString() };

      ViewBag.Page = 1;
    }
  }
}