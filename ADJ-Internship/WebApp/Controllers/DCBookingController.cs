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
    public async Task<ActionResult> Index()
    {
      PagedListResult<DCBookingDtos> pagedListResult = await _dCBookingService.ListDCBookingDtosAsync();
      return View();
    }
  }
}