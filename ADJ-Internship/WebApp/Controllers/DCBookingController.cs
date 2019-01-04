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
  public class DCBookingController : Controller
  {
    private readonly IDCBookingService _dCBookingService;
    public DCBookingController(IDCBookingService dCBookingService)
    {
      _dCBookingService = dCBookingService;
    }
    // GET: DCBooking
    public async Task<ActionResult> Index(int? pageIndex,string DestinationPort=null,string bookingref=null,DateTime? bookingdatefrom=null,DateTime? bookingdateto=null,string DC=null,DateTime? arrivaldatefrom=null,DateTime? arrivaldateto=null,string Status=null,string Container=null,bool checkClick=false)
    {
      ViewBag.DC = new List<string> { "Market Hong Kong", "Gas Customer Center","JSI Logistics" };
      ViewBag.Haulier = new List<string> {"123 Cargo","Cargo Core" };
      int current = pageIndex ?? 1;
      PagedListResult<DCBookingDtos> pagedListResult = await _dCBookingService.ListDCBookingDtosAsync(current,10,DestinationPort,bookingref,bookingdatefrom,bookingdateto,DC,arrivaldatefrom,arrivaldateto,Status,Container);
      if (checkClick == true)
      {
        return PartialView("_SearchingDCBookingPartial", pagedListResult);
      }
      return View(pagedListResult);
    }
  }
}