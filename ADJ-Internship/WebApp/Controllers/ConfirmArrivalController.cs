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
    private readonly int pageSize;

    public ConfirmArrivalController(IConfirmArrivalService CAService)
    {
      _CAService = CAService;
      pageSize = 6;
    }

    public async Task<ActionResult> Index()
    {
      SetDropDownList();
      ConfirmArrivalDtos model = new ConfirmArrivalDtos();
      model.Containers = new List<ConfirmArrivalResultDtos>();

      model.Containers = await _CAService.ListContainerFilterAsync(null, null, null, "HongKong", null, null, null, null);
      
      if (model.Containers.Count == 0)
      {
        ViewBag.ShowModal = "NoResult";
      }

      return View(model);
    }

    public async Task<ActionResult> Search(int? page, ConfirmArrivalDtos model)
    {
      SetDropDownList();

      model.Containers = new List<ConfirmArrivalResultDtos>();

      model.Containers = await _CAService.ListContainerFilterAsync(page, model.FilterDtos.ETAFrom, model.FilterDtos.ETATo, model.FilterDtos.Origin,
        model.FilterDtos.Mode, model.FilterDtos.Vendor, model.FilterDtos.Container, model.FilterDtos.Status);

      if (model.Containers.Count == 0)
      {
        ViewBag.ShowModal = "NoResult";
      }

      return PartialView("_Result", model);
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