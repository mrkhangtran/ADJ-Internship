using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
  public class ShipmentBookingController : Controller
  {
    private readonly IShipmentBookingService _bookingService;

    public ShipmentBookingController(IShipmentBookingService bookingService)
    {
      _bookingService = bookingService;
    }

    public async Task<ActionResult> Index()
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
      model.OrderDetails = new List<ShipmentResult>();

      model.OrderDetails = await _bookingService.ConvertToResult(await _bookingService.ListShipmentFilterAsync(page, origin, originPort, mode, warehouse, status, vendor, poNumber, itemNumber));

      return PartialView("_Result", model);
    }

    public void SetDropDownList()
    {
      ViewBag.Modes = new List<string> { "Road", "Sea", "Air" };
      ViewBag.PackTypes = new List<string> { "Boxed", "Carton" };
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };
      ViewBag.Ports = new List<string> { "Cẩm Phả", "Cửa Lò", "Hải Phòng", "Hòn Gai", "Nghi Sơn"  };
      ViewBag.Statuses = new List<string> { "AwaitingBooking", "BookingMade" };
    }
  }
}