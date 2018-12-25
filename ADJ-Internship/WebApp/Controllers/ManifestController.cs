using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ManifestController : Controller
    {
        // GET: Manifest
        public ActionResult Index()
        {
            return View();
        }
    }
}