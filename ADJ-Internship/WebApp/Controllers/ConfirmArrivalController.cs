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
      model.FilterDtos = new ConfirmArrivalFilterDtos();
      model.Containers = new PagedListResult<ConfirmArrivalResultDtos>();

      model.Containers = await _CAService.ListContainerFilterAsync(null, null, null, "HongKong", null, null, null, null);

      if (model.Containers.Items.Count == 0)
      {
        ViewBag.ShowModal = "NoResult";
      }

      PagedListResult<ConfirmArrivalResultDtos> nextPage = new PagedListResult<ConfirmArrivalResultDtos>();
      nextPage = await _CAService.ListContainerFilterAsync(2, null, null, "HongKong", null, null, null, null);

      if ((model.Containers.Items.Count > 0) && (nextPage.Items.Count > 0))
      {
        if (SameGroup(model.Containers.Items[model.Containers.Items.Count - 1], nextPage.Items[0]))
        {
          ViewBag.ToBeContinued = true;
        }
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

      PagedListResult<ConfirmArrivalResultDtos> nextPage = new PagedListResult<ConfirmArrivalResultDtos>();
      PagedListResult<ConfirmArrivalResultDtos> previousPage = new PagedListResult<ConfirmArrivalResultDtos>();
      previousPage.Items = new List<ConfirmArrivalResultDtos>();

      if (pageIndex - 1 > 0)
      {
        previousPage = await _CAService.ListContainerFilterAsync(pageIndex - 1, model.FilterDtos.ETAFrom, model.FilterDtos.ETATo, model.FilterDtos.Origin,
        model.FilterDtos.Mode, model.FilterDtos.Vendor, model.FilterDtos.Container, model.FilterDtos.Status);
      }
      nextPage = await _CAService.ListContainerFilterAsync(pageIndex + 1, model.FilterDtos.ETAFrom, model.FilterDtos.ETATo, model.FilterDtos.Origin,
        model.FilterDtos.Mode, model.FilterDtos.Vendor, model.FilterDtos.Container, model.FilterDtos.Status);

      if ((model.Containers.Items.Count > 0) && (nextPage.Items.Count > 0))
      {
        if (SameGroup(model.Containers.Items[model.Containers.Items.Count - 1], nextPage.Items[0]))
        {
          ViewBag.ToBeContinued = true;
        }
      }

      if ((model.Containers.Items.Count > 0) && (previousPage.Items.Count > 0))
      {
        if (SameGroup(model.Containers.Items[0], previousPage.Items[previousPage.Items.Count - 1]))
        {
          ViewBag.ContinuedFromPrevious = true;
        }
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
                if (item.Selected)
                {
                  await _CAService.CreateOrUpdateCAAsync(item.Id, model.ListArrivalDate[item.GroupId]);
                  item.ArrivalDate = model.ListArrivalDate[item.GroupId];
                  item.Status = ContainerStatus.Arrived;
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

    public bool SameGroup(ConfirmArrivalResultDtos first, ConfirmArrivalResultDtos second)
    {
      for (int property = 0; property < 5; property++)
      {
        var currentProperty = typeof(ConfirmArrivalResultDtos).GetProperties()[property];
        string firstValue = currentProperty.GetValue(first).ToString();
        string secondValue = currentProperty.GetValue(second).ToString();

        if (firstValue.CompareTo(secondValue) != 0)
        {
          return false;
        }
      }

      return true;
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

      ViewBag.ContinuedFromPrevious = false;
      ViewBag.ToBeContinued = false;

      ViewBag.Page = 1;
    }
  }
}