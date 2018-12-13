using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADJ.WebApp.Controllers
{
    public class PurchaseOrderController : Controller
    {

        private readonly IPurchaseOrderService _poService;

        public PurchaseOrderController(IPurchaseOrderService poService)
        {
            _poService = poService;
        }

        public ActionResult Index(string id, int? method)
        {
            if ((id != null) && (method != null))
            {
                string message = "The Purchase Order number " + id.ToString() + " is ";
                switch (method)
                {
                    case 1:
                        message += "created";
                        break;
                    case 2:
                        message += "edited";
                        break;
                    case 3:
                        message += "copied";
                        break;

                }
                message += " successfully.Thank you.";
                ViewBag.Message = message;
            }

            return View();
        }

        //Create New Order
        public ActionResult Create()
        {
            ViewBag.Method = "Create Order";
            OrderDTO defaultModel = new OrderDTO();
            defaultModel.orderDetailDTO = new OrderDetailDTO();
            defaultModel.orderDetailDTO.ItemNumber = "";
            defaultModel = SetDropDownList(defaultModel);
            ViewBag.ItemId = -2;

            return View(defaultModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(OrderDTO addModel, string method)
        {
            addModel = SetDropDownList(addModel);

            switch (method)
            {
                case "Apply":
                    if (addModel.PODetails == null)
                    {
                        ViewBag.OrderDetailError = "Please add at least 1 item detail.";
                        ViewBag.ItemId = -1;
                        return View(addModel);
                    }

                    if (!(await _poService.UniquePONumAsync(addModel.PONumber, addModel.Id)))
                    {
                        ViewBag.PONumberError = "PO Number must be unique.";
                        return View(addModel);
                    }

                    if (ModelState.IsValid)
                    {
                        await _poService.CreateOrUpdateOrderAsync(addModel);
                        foreach (var i in addModel.PODetails.Items)
                        {
                            i.OrderId = await _poService.GetLastOrderId();
                            await _poService.CreateOrUpdateOrderDetailAsync(i);
                        }

                        return RedirectToAction("Index", new { id = addModel.PONumber, method = 1 });
                    }
                    break;
                case "Save":
                    if (addModel.PODetails == null)
                    {
                        addModel.PODetails = new PagedListResult<OrderDetailDTO>();
                        addModel.PODetails.Items = new List<OrderDetailDTO>();
                    }

                    if ((!UniqueItemNumber(-1, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
                    {
                        ViewBag.ItemNumberError = "Item Number must be unique.";
                        ViewBag.ItemId = -1;
                        return View(addModel);
                    }

                    addModel.PODetails.Items.Add(addModel.orderDetailDTO);
                    ViewBag.ItemId = -2;
                    break;
                case "AddItem":
                    ViewBag.ItemId = -1;
                    break;
                default:
                    int itemId = int.Parse(new string(method.Where(char.IsDigit).ToArray()));
                    ModelState.Clear();
                    if (method.Contains("Update"))
                    {
                        if ((!UniqueItemNumber(itemId, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
                        {
                            ViewBag.ItemNumberError = "Item Number must be unique.";
                            ViewBag.ItemId = itemId;
                            return View(addModel);
                        }

                        addModel.PODetails.Items[itemId] = addModel.orderDetailDTO;
                        ViewBag.ItemId = -2;
                    }
                    else if (method.Contains("Delete"))
                    {
                        addModel.PODetails.Items.Remove(addModel.PODetails.Items[itemId]);
                    }
                    else
                    {
                        addModel.orderDetailDTO = addModel.PODetails.Items[itemId];
                        ViewBag.ItemId = itemId;
                    }
                    break;
            }

            return View(addModel);
        }

        public async Task<ActionResult> Edit(string PONumber)
        {
            OrderDTO editModel = new OrderDTO();
            editModel = await _poService.GetOrderByPONumber(PONumber);

            if (editModel.Id == 0)
            {
                return StatusCode(404);
            }

            editModel.orderDetailDTO = new OrderDetailDTO();
            editModel.orderDetailDTO.OrderId = editModel.Id;
            editModel.orderDetailDTO.ItemNumber = editModel.PODetails.Items[0].ItemNumber;
            ViewBag.Method = "Edit Order Number: " + editModel.PONumber;
            editModel = SetDropDownList(editModel);
            ViewBag.ItemId = -2;

            return View(editModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(OrderDTO addModel, string method)
        {
            ViewBag.Method = "Edit Order Number: " + addModel.PONumber;
            addModel = SetDropDownList(addModel);

            switch (method)
            {
                case "Apply":
                    if (addModel.PODetails == null)
                    {
                        ViewBag.OrderDetailError = "Please add at least 1 item detail.";
                        ViewBag.ItemId = -1;
                        return View(addModel);
                    }

                    if (!(await _poService.UniquePONumAsync(addModel.PONumber, addModel.Id)))
                    {
                        ViewBag.PONumberError = "PO Number must be unique.";
                        return View(addModel);
                    }

                    if (ModelState.IsValid)
                    {
                        await _poService.CreateOrUpdateOrderAsync(addModel);
                        _poService.DeleteOrderDetailAsync(addModel.PODetails, addModel.Id);
                        foreach (var i in addModel.PODetails.Items)
                        {
                            i.OrderId = addModel.Id;
                            await _poService.CreateOrUpdateOrderDetailAsync(i);
                        }

                        return RedirectToAction("Index", new { id = addModel.PONumber, method = 2 });
                    }
                    break;
                case "Save":
                    if (addModel.PODetails == null)
                    {
                        addModel.PODetails = new PagedListResult<OrderDetailDTO>();
                        addModel.PODetails.Items = new List<OrderDetailDTO>();
                    }

                    if ((!UniqueItemNumber(-1, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
                    {
                        ViewBag.ItemNumberError = "Item Number must be unique.";
                        ViewBag.ItemId = -1;
                        return View(addModel);
                    }

                    addModel.PODetails.Items.Add(addModel.orderDetailDTO);
                    ViewBag.ItemId = -2;
                    break;
                case "AddItem":
                    ViewBag.ItemId = -1;
                    break;
                default:
                    int itemId = int.Parse(new string(method.Where(char.IsDigit).ToArray()));
                    ModelState.Clear();
                    if (method.Contains("Update"))
                    {
                        if ((!UniqueItemNumber(itemId, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
                        {
                            ViewBag.ItemNumberError = "Item Number must be unique.";
                            ViewBag.ItemId = itemId;
                            return View(addModel);
                        }

                        addModel.PODetails.Items[itemId] = addModel.orderDetailDTO;
                        ViewBag.ItemId = -2;
                    }
                    else if (method.Contains("Delete"))
                    {
                        addModel.PODetails.Items.Remove(addModel.PODetails.Items[itemId]);
                    }
                    else
                    {
                        addModel.orderDetailDTO = addModel.PODetails.Items[itemId];
                        ViewBag.ItemId = itemId;
                    }
                    break;
            }

            return View(addModel);
        }

        public async Task<ActionResult> Copy(string PONumber, int? page)
        {
            ModelState.Clear();
            OrderDTO copyModel = new OrderDTO();
            copyModel = await _poService.GetOrderByPONumber(PONumber);

            if (copyModel.Id == 0)
            {
                return StatusCode(404);
            }

            copyModel.Id = 0;
            
            copyModel.OrderDate = DateTime.Now;
            copyModel.ShipDate = DateTime.Now;
            copyModel.LatestShipDate = DateTime.Now;
            copyModel.DeliveryDate = DateTime.Now;
            copyModel.PODetails = new PagedListResult<OrderDetailDTO>();
            copyModel.PODetails.Items = new List<OrderDetailDTO>();
            copyModel.PODetails.PageCount = 1;
            copyModel.PODetails.TotalCount = 5;
            copyModel.orderDetailDTO = new OrderDetailDTO();
            copyModel = SetDropDownList(copyModel);

            ViewBag.Method = "Copy from Order Number: " + copyModel.PONumber;
            copyModel.PONumber = "";
            ViewBag.ItemId = -2;

            return View(copyModel);
        }

        [HttpPost]
        public async Task<ActionResult> Copy(OrderDTO addModel, string method, int? page)
        {
            addModel = SetDropDownList(addModel);

            switch (method)
            {
                case "Apply":
                    if (addModel.PODetails == null)
                    {
                        ViewBag.OrderDetailError = "Please add at least 1 item detail.";
                        ViewBag.ItemId = -1;
                        return View(addModel);
                    }

                    if (!(await _poService.UniquePONumAsync(addModel.PONumber, addModel.Id)))
                    {
                        ViewBag.PONumberError = "PO Number must be unique.";
                        return View(addModel);
                    }

                    if (ModelState.IsValid)
                    {
                        await _poService.CreateOrUpdateOrderAsync(addModel);
                        foreach (var i in addModel.PODetails.Items)
                        {
                            i.OrderId = await _poService.GetLastOrderId();
                            await _poService.CreateOrUpdateOrderDetailAsync(i);
                        }

                        return RedirectToAction("Index", new { id = addModel.PONumber, method = 3 });
                    }
                    break;
                case "Save":
                    if (addModel.PODetails == null)
                    {
                        addModel.PODetails = new PagedListResult<OrderDetailDTO>();
                        addModel.PODetails.Items = new List<OrderDetailDTO>();
                    }

                    if ((!UniqueItemNumber(-1, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
                    {
                        ViewBag.ItemNumberError = "Item Number must be unique.";
                        ViewBag.ItemId = -1;
                        return View(addModel);
                    }

                    addModel.PODetails.Items.Add(addModel.orderDetailDTO);
                    ViewBag.ItemId = -2;
                    break;
                case "AddItem":
                    ViewBag.ItemId = -1;
                    break;
                default:
                    int itemId = int.Parse(new string(method.Where(char.IsDigit).ToArray()));
                    ModelState.Clear();
                    if (method.Contains("Update"))
                    {
                        if ((!UniqueItemNumber(itemId, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
                        {
                            ViewBag.ItemNumberError = "Item Number must be unique.";
                            ViewBag.ItemId = itemId;
                            return View(addModel);
                        }

                        addModel.PODetails.Items[itemId] = addModel.orderDetailDTO;
                        ViewBag.ItemId = -2;
                    }
                    else if (method.Contains("Delete"))
                    {
                        addModel.PODetails.Items.Remove(addModel.PODetails.Items[itemId]);
                    }
                    else
                    {
                        addModel.orderDetailDTO = addModel.PODetails.Items[itemId];
                        ViewBag.ItemId = itemId;
                    }
                    break;
            }

            return View(addModel);
        }

        private bool UniqueItemNumber(int id, string itemNum, List<OrderDetailDTO> orderDetails)
        {
            if (id == -1)
            {
                foreach (var item in orderDetails)
                {
                    if (itemNum == item.ItemNumber) { return false; }
                }
            }
            else
            {
                for (var i=0; i<orderDetails.Count(); i++)
                {
                    if ((itemNum == orderDetails[i].ItemNumber) && (id != i)) { return false; }
                }
            }

            return true;
        }

        //Set DropDownList to select on View
        private OrderDTO SetDropDownList(OrderDTO addModel)
        {
            OrderDTO init = new OrderDTO();
            init = addModel;

            init.Seasons = GetSelectListItems(SeasonList());
            init.Ports = GetSelectListItems(new List<string> { "Cẩm Phả", "Hòn Gai", "Hải Phòng", "Nghi Sơn", "Cửa Lò," });
            init.Modes = GetSelectListItems(new List<string> { "Road", "Sea", "Air" });
            init.Origins = GetSelectListItems(new List<string> { "HongKong", "Vietnam" });
            
            //Currency
            List<string> temp = new List<string>();

            foreach (var i in Enum.GetValues(typeof(Currency)))
            {
                temp.Add(i.ToString());
            }

            init.Currencies = GetSelectListItems(temp);

            //Status
            temp = new List<string>();

            foreach (var i in Enum.GetValues(typeof(OrderStatus)))
            {
                temp.Add(i.ToString());
            }

            init.Statuses = GetSelectListItems(temp);

            //Port
            //temp = new List<string>();

            //foreach (var i in Enum.GetValues(typeof(Ports)))
            //{
            //    temp.Add(i.ToString());
            //}

            //init.Ports = GetSelectListItems(temp);

            ////Mode
            //temp = new List<string>();

            //foreach (var i in Enum.GetValues(typeof(Modes)))
            //{
            //    temp.Add(i.ToString());
            //}

            //init.Modes = GetSelectListItems(temp);

            return init;
        }

        //Set list of years for Season field
        private IEnumerable<string> SeasonList()
        {
            List<string> seasonList = new List<string>();
            for (int i = 2010; i <= 2020; i++)
            {
                seasonList.Add(i.ToString());
            }
            return seasonList;
        }

        //Convert string list to SelectListItem list
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }
    }
}