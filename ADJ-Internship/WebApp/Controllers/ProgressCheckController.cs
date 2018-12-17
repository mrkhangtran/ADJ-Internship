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
    public class ProgressCheckController : Controller
    {
        private readonly IProgressCheckService _prcService;
        public ProgressCheckController(IProgressCheckService prcService)
        {
            _prcService = prcService;
        }
        // GET: ProgressCheckDto
        public async Task<ActionResult> Index(string PONumberSearch = null, string ItemSearch = null, string Suppliers = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null)
        {
            GetItemSearchDto getSearchItem = await _prcService.SearchItem();
            ViewBag.Suppliers = getSearchItem.Suppliers;
            ViewBag.Origins = getSearchItem.Origins;
            ViewBag.OriginPorts = getSearchItem.OriginPorts;
            ViewBag.Factories = getSearchItem.Factories;
            ViewBag.Depts = getSearchItem.Depts;
            PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync();
            List<ProgressCheckDto> progressCheckDtos = lstPrc.Items;        
            return View("Index",progressCheckDtos);
        }
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdate(List<ProgressCheckDto> progressCheckDTOs,List<string> POList,List<string> ItemList)
        {
            if (ModelState.IsValid)
            {

                for (int i = 0; i < progressCheckDTOs.Count(); i++)
                {
                    for (int j = 0; j < POList.Count; j++)
                    {
                        if (progressCheckDTOs[i].PONumber == POList[j])
                        {
                            await _prcService.CreateOrUpdatePurchaseOrderAsync(progressCheckDTOs[i]);
                        }
                    }
                }               
            }
            GetItemSearchDto getSearchItem = await _prcService.SearchItem();
            ViewBag.Suppliers = getSearchItem.Suppliers;
            ViewBag.Origins = getSearchItem.Origins;
            ViewBag.OriginPorts = getSearchItem.OriginPorts;
            ViewBag.Factories = getSearchItem.Factories;
            ViewBag.Depts = getSearchItem.Depts;
            PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync();
            List<ProgressCheckDto> progressCheckDtos = lstPrc.Items;
            return RedirectToAction("Index", progressCheckDTOs);
        }
        public async Task<ActionResult> Searching(string PONumberSearch = null, string ItemSearch = null, string Suppliers = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null)
        {
            PagedListResult<ProgressCheckDto> lstPrc = await _prcService.ListProgressCheckDtoAsync();
            List<ProgressCheckDto> progressCheckDtos = lstPrc.Items;
            if (PONumberSearch != null)
            {
                progressCheckDtos = progressCheckDtos.Where(p => p.PONumber == PONumberSearch).ToList();
            }
            if (!String.IsNullOrEmpty(Suppliers))
            {
                progressCheckDtos = progressCheckDtos.Where(p => p.Supplier == Suppliers).ToList();
            }
            if (!String.IsNullOrEmpty(Factories))
            {
                progressCheckDtos = progressCheckDtos.Where(p => p.Factory == Factories).ToList();
            }
            if (!String.IsNullOrEmpty(Origins))
            {
                progressCheckDtos = progressCheckDtos.Where(p => p.Origin == Origins).ToList();
            }
            if (!String.IsNullOrEmpty(OriginPorts))
            {
                progressCheckDtos = progressCheckDtos.Where(p => p.OriginPort == OriginPorts).ToList();
            }
            if (!String.IsNullOrEmpty(ItemSearch))
            {
                for (int i = 0; i < progressCheckDtos.Count; i++)
                {
                    int check = 0;
                    foreach (var item in progressCheckDtos[i].ListOrderDetailDto)
                    {
                        if (item.ItemNumber == ItemSearch)
                        {
                            check += 1;
                        }
                    }
                    if (check == 0)
                    {
                        progressCheckDtos.Remove(progressCheckDtos[i]);
                    }
                }
            }
            List<ProgressCheckDto> model = progressCheckDtos;
            return PartialView("_SearchingPartial", model);
        }
    }
}