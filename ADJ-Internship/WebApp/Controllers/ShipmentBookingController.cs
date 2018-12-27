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

    public ActionResult Index()
    {
      SetDropDownList();
      ShipmentBookingDtos model = new ShipmentBookingDtos();

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
              if (SelectAtLeastOne(model.OrderDetails))
              {
                {
                  await _bookingService.CreateOrUpdateBookingAsync(model);
                  ModelState.Clear();
                  model = await _bookingService.ChangeItemStatus(model);
                  ViewBag.ShowModal = "Updated";
                }
              }
              else
              {
                ViewBag.ShowModal = "NoItem";
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
      ViewBag.Modes = new List<string> { "Road", "Sea", "Air" };
      ViewBag.PackTypes = new List<string> { "Boxed", "Carton" };
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };
      ViewBag.Ports = new List<string> { "Cẩm Phả", "Cửa Lò", "Hải Phòng", "Hòn Gai", "Nghi Sơn" };
      ViewBag.Statuses = new List<string> { "AwaitingBooking", "BookingMade" };
      ViewBag.Carriers = new List<string> { "DHL", "EMS", "Kerry Express", "TNT", "USPS", "ViettelPost" };

      ViewBag.PageSize = pageSize;
      ViewBag.Page = 1;
    }
  }
}