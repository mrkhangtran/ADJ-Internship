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
  public class DCReceivedController : Controller
  {
    private readonly IDCReceivedService _dcReceivedService;

    public DCReceivedController(IDCReceivedService dcReceivedService)
    {
      _dcReceivedService = dcReceivedService;
    }

    public async Task<ActionResult> Index()
    {
      SetDropDownList();

      DCReceivedDtos model = new DCReceivedDtos();
      model.FilterDtos = new DCReceivedFilterDtos();
      model.ResultDtos = new PagedListResult<DCReceivedResultDtos>();

      model.ResultDtos = await _dcReceivedService.ListContainerFilterAsync(null, null, null, null, null, null, null, null, null);

      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Search(DCReceivedDtos model, string page = null)
    {
      SetDropDownList();
      if (page == null) { page = "1"; }
      int pageIndex = int.Parse(page);
      ViewBag.Page = pageIndex;

      model.ResultDtos = new PagedListResult<DCReceivedResultDtos>();

      model.ResultDtos = await _dcReceivedService.ListContainerFilterAsync(pageIndex, model.FilterDtos.Container, model.FilterDtos.DC, model.FilterDtos.BookingDateFrom, 
        model.FilterDtos.BookingDateTo, model.FilterDtos.DeliveryDateFrom, model.FilterDtos.DeliveryDateTo, model.FilterDtos.BookingRef, model.FilterDtos.Status);

      return PartialView("_Result", model);
    }

    public async Task<ActionResult> Achieve(DCReceivedDtos model)
    {
      SetDropDownList();

      if (ModelState.IsValid)
      {
        if (model.ResultDtos != null)
        {
          if (SelectAtLeastOne(model.ResultDtos.Items))
          {
            {
              foreach (var item in model.ResultDtos.Items)
              {
                if (item.Selected)
                {
                  await _dcReceivedService.CreateOrUpdateCAAsync(item);
                  item.Status = ContainerStatus.Delivered;
                  item.StatusDescription = ContainerStatus.Delivered.GetDescription<ContainerStatus>();
                }
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

    public bool SelectAtLeastOne(List<DCReceivedResultDtos> input)
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
      ViewBag.DCs = new List<string> { "Market Hong Kong", "Gas Customer Center", "JSI Logistics" };
      ViewBag.Statuses = new List<string> { ContainerStatus.Despatch.GetDescription<ContainerStatus>(), ContainerStatus.Arrived.GetDescription<ContainerStatus>() };

      ViewBag.Page = 1;
    }
  }
}