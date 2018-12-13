using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ADJ.WebApp.Models;

namespace ADJ.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPurchaseOrderService _poService;
        private readonly IProgressCheckService _prcService;
        public HomeController(IPurchaseOrderService poService, IProgressCheckService prcService)
        {
            _poService = poService;
            _prcService = prcService;
         
        }

        public async Task<IActionResult> Index()
        {
            var createPurchaseOrderRq = new CreateOrUpdatePurchaseOrderRq {Test = "111"};
            await _prcService.CheckOrderHaventProgress(1);
            _prcService.CreateDefaultModel(1);
            await _poService.CreateOrUpdatePurchaseOrderAsync(createPurchaseOrderRq);

            var test = await _poService.ListPurchaseOrdersAsync(null);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
