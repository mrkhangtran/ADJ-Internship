using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Implementations;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Controllers
{
  public class OrderController : Controller
  {
    private readonly IOrderDisplayService _orderService;
    private readonly int pageSize;

    public OrderController(IOrderDisplayService orderService)
    {
      _orderService = orderService;
      pageSize = 20;
    }

    public async Task<IActionResult> Display(string poNumberFilter, int? pageIndex)
    {
      int current = pageIndex ?? 1;
      PagedListResult<OrderDTO> pagedlistResult = await _orderService.DisplaysAsync(poNumberFilter, current, pageSize);
      ViewBag.CurrentPage = current;

      return View(pagedlistResult);
    }
  }
}