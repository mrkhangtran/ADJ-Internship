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
  public class ManifestController : Controller
  {
    private readonly IManifestService _manifestService;
    public ManifestController(IManifestService manifestService)
    {
      _manifestService = manifestService;
    }
    // GET: Manifest
    public async Task<ActionResult> Index()
    {
      PagedListResult<ShipmentManifestsDtos> listManifest = await _manifestService.ListManifestDtoAsync();

      return View(listManifest);
    }
  }
}