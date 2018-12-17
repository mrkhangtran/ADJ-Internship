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

		public OrderController(IOrderDisplayService orderService)
		{
			_orderService = orderService;
		}


		public async Task<PartialViewResult> Display( string poNumber)
		{
			List<OrderDto> listPO = await _orderService.DisplaysAsync(poNumber);

			if (listPO.Count == 0)
			{
				ViewBag.Massage = "There is no available PO";
				return PartialView(listPO);
			}

			return PartialView(listPO);
		}


	}
}