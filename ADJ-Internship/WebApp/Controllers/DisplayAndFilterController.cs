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
    public class DisplayAndFilterController : Controller
    {
        private readonly IDisplayAndFilterService _displayAndFilterService;

        public DisplayAndFilterController(IDisplayAndFilterService displayAndFilterService)
        {
            _displayAndFilterService = displayAndFilterService;
        }

        //Display
        public async Task<IActionResult> Display( )
        {
            List<PODisplayDto> lstPO = await _displayAndFilterService.GetPOsAsync();

            //List<PODisplayDto> lstResult = new List<PODisplayDto>;

            //int pageNumber = (lstPO.Count / 20) + 1;
            //for(int i = 1; i <= 20; i++)
            //{
            //    foreach(var item in lstPO)
            //    {
            //        lstResult.Add(item);
            //    }
            //}
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
            List<PODisplayDto> lstPO = await _displayAndFilterService.GetPOsAsync();
            List<PODisplayDto> lstFilterResult = await _displayAndFilterService.FilterPO(sKey);
            if (lstFilterResult.Count == 0)
            {
                ViewBag.Message = "No match result, please try again";
                return View(lstPO.OrderByDescending(n => n.PODate));
            }

            return View(lstFilterResult);
        }
       

    }
}