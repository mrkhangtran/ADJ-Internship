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
    private readonly IVesselDepartureService _vesselDepartureService;
    private readonly int pageSize;

    public VesselDepartureController(IVesselDepartureService vesselDepartureService)
    {
      _vesselDepartureService = vesselDepartureService;
      pageSize = 3;
    }

    public async Task<ActionResult> Index()
    {
      SetDropDownList();
      VesselDepartureDtos model = new VesselDepartureDtos();
      model.FilterDto = new FilterDto();
      model.ResultDtos = new PagedListResult<ContainerDto>();

      model.ResultDtos = await _vesselDepartureService.ListContainerDtoAsync(null, null, null, null, null, null, null);

      if (model.ResultDtos.Items.Count == 0)
      {
        ViewBag.ShowModal = "NoResult";
      }

      PagedListResult<ContainerDto> nextPage = new PagedListResult<ContainerDto>();
      nextPage = await _vesselDepartureService.ListContainerDtoAsync(2, null, null, null, null, null, null);

      if ((model.ResultDtos.Items.Count > 0) && (nextPage.Items.Count > 0))
      {
        if (SameGroup(model.ResultDtos.Items[model.ResultDtos.Items.Count - 1], nextPage.Items[0]))
        {
          ViewBag.ToBeContinued = true;
        }
      }

      return View("Index", model);
    }

    [HttpPost]
    public async Task<ActionResult> Search(VesselDepartureDtos model, string page = null)
    {
      SetDropDownList();
      if (page == null) { page = "1"; }
      int pageIndex = int.Parse(page);
      ViewBag.Page = pageIndex;

      model.ResultDtos = new PagedListResult<ContainerDto>();

      model.ResultDtos = await _vesselDepartureService.ListContainerDtoAsync(pageIndex, model.FilterDto.Origin, model.FilterDto.OriginPort, model.FilterDto.Container, 
        model.FilterDto.Status, model.FilterDto.ETDFrom, model.FilterDto.ETDTo);

      if (model.ResultDtos.Items.Count == 0)
      {
        ViewBag.ShowModal = "NoResult";
      }

      PagedListResult<ContainerDto> nextPage = new PagedListResult<ContainerDto>();
      PagedListResult<ContainerDto> previousPage = new PagedListResult<ContainerDto>();

      if (pageIndex - 1 > 0)
      {
        previousPage = await _vesselDepartureService.ListContainerDtoAsync(pageIndex - 1, model.FilterDto.Origin, model.FilterDto.OriginPort, model.FilterDto.Container,
        model.FilterDto.Status, model.FilterDto.ETDFrom, model.FilterDto.ETDTo);
      }
      nextPage = await _vesselDepartureService.ListContainerDtoAsync(pageIndex + 1, model.FilterDto.Origin, model.FilterDto.OriginPort, model.FilterDto.Container,
        model.FilterDto.Status, model.FilterDto.ETDFrom, model.FilterDto.ETDTo);

      if ((model.ResultDtos.Items.Count > 0) && (nextPage.Items.Count > 0))
      {
        if (SameGroup(model.ResultDtos.Items[model.ResultDtos.Items.Count - 1], nextPage.Items[0]))
        {
          ViewBag.ToBeContinued = true;
        }
      }

      if ((model.ResultDtos.Items.Count > 0) && (previousPage.Items.Count > 0))
      {
        if (SameGroup(model.ResultDtos.Items[0], previousPage.Items[previousPage.Items.Count - 1]))
        {
          ViewBag.ContinuedFromPrevious = true;
        }
      }

      return PartialView("_Result", model);
    }

    public bool SameGroup(ContainerDto first, ContainerDto second)
    {
      for (int property = 0; property < 6; property++)
      {
        var currentProperty = typeof(ContainerDto).GetProperties()[property];
        string firstValue = currentProperty.GetValue(first).ToString();
        string secondValue = currentProperty.GetValue(second).ToString();

        if (firstValue.CompareTo(secondValue) != 0)
        {
          return false;
        }
      }

      return true;
    }

    public void SetDropDownList()
    {
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };
      ViewBag.Modes = new List<string> { "Road", "Sea", "Air" };
      ViewBag.Statuses = new List<string> { ContainerStatus.Despatch.ToString(), ContainerStatus.Pending.ToString() };

      ViewBag.Page = 1;
    }

  }
}