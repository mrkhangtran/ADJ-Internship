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
  public class ShipmentBookingController : Controller
  {
    private readonly IShipmentBookingService _bookingService;
    private readonly int pageSize;

    public ShipmentBookingController(IShipmentBookingService bookingService)
    {
      _bookingService = bookingService;
      pageSize = 6;
    }

    public async Task<ActionResult> Index()
    {
      SetDropDownList();
      ShipmentBookingDtos model = new ShipmentBookingDtos();
      model.OrderDetails = new List<ShipmentResultDtos>();

      model.OrderDetails = await _bookingService.ConvertToResultAsync(await _bookingService.ListShipmentFilterAsync(null, "HongKong", "Aberdeen", null, null, null, null, null, null));

      if (model.OrderDetails.Count == 0)
      {
        ViewBag.ShowModal = "NoResult";
      }

      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Filter(int? page, string origin = null, string originPort = null, string mode = null, string warehouse = null,
      string status = null, string vendor = null, string poNumber = null, string itemNumber = null)
    {
      SetDropDownList();

      ShipmentBookingDtos model = new ShipmentBookingDtos();
      model.OrderDetails = new List<ShipmentResultDtos>();

      model.OrderDetails = await _bookingService.ConvertToResultAsync(await _bookingService.ListShipmentFilterAsync(page, origin, originPort, mode, warehouse, status, vendor, poNumber, itemNumber));

      if (model.OrderDetails.Count > 0)
      {
        model.OrderDetails = await _bookingService.UpdatePackType(model.OrderDetails);
      }
      else
      {
        ViewBag.ShowModal = "NoResult";
      }

      return PartialView("_Result", model);
    }

    [HttpPost]
    public async Task<ActionResult> Booking(ShipmentBookingDtos model, string method = null)
    {
      SetDropDownList();

      switch (method)
      {
        case "Book":
          if (ModelState.IsValid)
          {
            if (model.OrderDetails != null)
            {
              if (SelectAtLeastOne(model.OrderDetails))
              {
                DateTime earliestShipDate = GetEarliestShipDate(model.OrderDetails);
                DateTime latestDeliveryDate = GetLatestDeliveryDate(model.OrderDetails, earliestShipDate);

                if ((earliestShipDate < model.ETD) && (model.ETA <= latestDeliveryDate))
                {
                  await _bookingService.CreateOrUpdateBookingAsync(model);
                  ModelState.Clear();
                  model = await _bookingService.ChangeItemStatus(model);
                  ViewBag.ShowModal = "Updated";
                }
                else
                {
                  if (earliestShipDate >= model.ETD)
                  {
                    ViewBag.ETDError = "Please set ETD after the date below.";
                  }
                  if (model.ETA > latestDeliveryDate)
                  {
                    ViewBag.ETAError = "Please set ETA no later than the date below.";
                  }
                }
              }
              else
              {
                ViewBag.ShowModal = "NoItem";
              }
            }
          }
          break;
        default:
          ModelState.Clear();
          int number = int.Parse(new string(method.Where(char.IsDigit).ToArray()));
          ViewBag.Page = number;
          break;
      }

      return PartialView("_Result", model);
    }

    public DateTime GetEarliestShipDate(List<ShipmentResultDtos> input)
    {
      DateTime earliest = DateTime.MaxValue;
      for (int i = 0; i < input.Count; i++)
      {
        if (input[i].POShipDate >= DateTime.Now)
        {
          if (input[i].POShipDate < earliest)
          {
            earliest = input[i].POShipDate;
          }
        }
      }

      if (earliest == DateTime.MaxValue)
      {
        earliest = DateTime.Now;
      }

      return earliest;
    }

    public DateTime GetLatestDeliveryDate(List<ShipmentResultDtos> input, DateTime shipDate)
    {
      DateTime latest = DateTime.MaxValue;
      for (int i = 0; i < input.Count; i++)
      {
        if (input[i].DeliveryDate >= shipDate.AddDays(2))
        {
          if (input[i].DeliveryDate < latest)
          {
            latest = input[i].DeliveryDate;
          }
        }
      }

      if (latest == DateTime.MaxValue)
      {
        latest = DateTime.Now.AddDays(2);
      }

      return latest;
    }

    public bool SelectAtLeastOne(List<ShipmentResultDtos> input)
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
      ViewBag.PackTypes = new List<string> { "Boxed", "Carton" };
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };
      ViewBag.VNPorts = new List<string> { "Cẩm Phả", "Cửa Lò", "Hải Phòng", "Hòn Gai", "Nghi Sơn" };
      ViewBag.HKPorts = new List<string> { "Aberdeen", "Crooked Harbour", "Double Haven", "Gin Drinkers Bay", "Inner Port Shelter" };
      ViewBag.Statuses = new List<string> { OrderStatus.AwaitingBooking.ToString(), OrderStatus.BookingMade.ToString() };
      ViewBag.Carriers = new List<string> { "DHL", "EMS", "Kerry Express", "TNT", "USPS", "ViettelPost" };

      ViewBag.PageSize = pageSize;
      ViewBag.Page = 1;
    }
  }
}