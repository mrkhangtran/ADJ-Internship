﻿using System;
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

      model.OrderDetails = await _bookingService.ConvertToResultAsync(await _bookingService.ListShipmentFilterAsync(null, "Hong Kong", "Aberdeen", null, null, null, null, null, null, null));

      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Filter(int? page, string origin = null, string originPort = null, string mode = null, string warehouse = null,
      string status = null, string vendor = null, string poNumber = null, string itemNumber = null, string shipmentId = null)
    {
      SetDropDownList();

      ShipmentBookingDtos model = new ShipmentBookingDtos();
      model.OrderDetails = new List<ShipmentResultDtos>();

      model.OrderDetails = await _bookingService.ConvertToResultAsync(await _bookingService.ListShipmentFilterAsync(page, origin, originPort, mode, warehouse, status, vendor, poNumber, itemNumber, shipmentId));

      if (model.OrderDetails.Count > 0)
      {
        model.OrderDetails = await _bookingService.UpdatePackType(model.OrderDetails);
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
                await _bookingService.CreateOrUpdateBookingAsync(model);
                ModelState.Clear();
                model = await _bookingService.ChangeItemStatus(model);
                ViewBag.ShowModal = "Updated";
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
      ViewBag.Modes = new List<string> { "Air", "Road", "Sea" };
      ViewBag.PackTypes = new List<string> { "Boxed", "Carton" };
      ViewBag.Origins = new List<string> { "Hong Kong", "Vietnam" };
      ViewBag.VNPorts = new List<string> { "Cam Pha", "Cua Lo", "Hai Phong", "Hon Gai", "Nghi Son" };
      ViewBag.HKPorts = new List<string> { "Aberdeen", "Crooked Harbour", "Double Haven", "Gin Drinkers Bay", "Inner Port Shelter" };
      ViewBag.Statuses = new List<string> { OrderStatus.AwaitingBooking.GetDescription<OrderStatus>(), OrderStatus.BookingMade.GetDescription<OrderStatus>() };
      ViewBag.Carriers = new List<string> { "DHL", "EMS", "Kerry Express", "TNT", "USPS", "ViettelPost" };

      ViewBag.PageSize = pageSize;
      ViewBag.Page = 1;
    }
  }
}