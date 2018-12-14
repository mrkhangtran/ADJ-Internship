using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Implementations;
using ADJ.BusinessService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _displayAndFilterService;

        public OrderController(IOrderService displayAndFilterService)
        {
            _displayAndFilterService = displayAndFilterService;
        }

        //Display
        public async Task<IActionResult> Display( )
        {
            List<OrderDisplayDto> lstPO = await _displayAndFilterService.GetPOsAsync();

            if (lstPO.Count == 0)
            {
                ViewBag.Massage = "There is no available PO";
                return View(lstPO.OrderByDescending(n => n.PODate));
            }

            return View(lstPO);
        }

        //Filter
        public async Task<IActionResult> FilterPO()
        {

            string sKey = HttpContext.Request.Form["filter"];
            List<OrderDisplayDto> lstPO = await _displayAndFilterService.GetPOsAsync();
            List<OrderDisplayDto> lstFilterResult = await _displayAndFilterService.FilterPO(sKey);
            if (lstFilterResult.Count == 0)
            {
                ViewBag.Message = "No match result, please try again";
                return View(lstPO.OrderByDescending(n => n.PODate));
            }

            return View(lstFilterResult);
        }
       

    }
}