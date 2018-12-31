using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.Common;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
  public class ConfirmArrivalController : Controller
  {
    public IActionResult Index()
    {
      SetDropDownList();

      return View();
    }

    public void SetDropDownList()
    {
      ViewBag.PackTypes = new List<string> { "Boxed", "Carton" };
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };
      ViewBag.VNPorts = new List<string> { "Cẩm Phả", "Cửa Lò", "Hải Phòng", "Hòn Gai", "Nghi Sơn" };
      ViewBag.HKPorts = new List<string> { "Aberdeen", "Crooked Harbour", "Double Haven", "Gin Drinkers Bay", "Inner Port Shelter" };
      ViewBag.Statuses = new List<string> { ContainerStatus.Despatch.ToString(), ContainerStatus.Arrived.ToString() };

      ViewBag.Page = 1;
    }
  }
}