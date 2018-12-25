using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


      return View();
    }

    public void SetDropDownList()
    {
      ViewBag.Modes = new List<string> { "Road", "Sea", "Air" };
      ViewBag.PackType = new List<string> { "Boxed", "Carton" };
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };
    }
  }
}