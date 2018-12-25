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
    private int pageSize = 5;

    public PurchaseOrderController(IPurchaseOrderService poService)
    {
      _poService = poService;
    }

    public ActionResult Index(string id, string method)
    {
      if ((id != null) && (method != null))
      {
        string message = "The Purchase Order number " + id.ToString() + " is ";
        switch (method)
        {
          case "Create":
            message += "created";
            break;
          case "Edit":
            message += "edited";
            break;
          case "Copy":
            message += "copied";
            break;

        }
        message += " successfully. Thank you.";
        ViewBag.Message = message;
      }

      return View();
    }

    //Create New Order
    public ActionResult Create()
    {
      OrderDTO defaultModel = new OrderDTO();
      defaultModel.Method = "Create New Order";
      defaultModel.SingleOrderDetail = new OrderDetailDTO();
      defaultModel.SingleOrderDetail.ItemNumber = "";
      defaultModel.PODetails = new PagedListResult<OrderDetailDTO>();
      defaultModel.PODetails.Items = new List<OrderDetailDTO>();
      SetDropDownList();
      ViewBag.ItemId = -2;
      ViewBag.PageSize = pageSize;

      return View(defaultModel);
    }

    [HttpPost]
    public async Task<ActionResult> Apply(OrderDTO addModel)
    {
      SetDropDownList();
      string viewName = addModel.Method.Substring(0, addModel.Method.IndexOf(" "));
      ViewBag.Method = addModel.Method;
      ViewBag.PageSize = pageSize;


      if ((addModel.PODetails == null) || (addModel.PODetails.Items == null))
      {
        ViewBag.OrderDetailError = "Please add at least 1 item.";
        ViewBag.ItemId = -1;
        return View(viewName, addModel);
      }
      else { ModelState["SingleOrderDetail.ItemNumber"].ValidationState = ModelState["Id"].ValidationState; }

      if (!(await _poService.UniquePONumAsync(addModel.PONumber, addModel.Id)))
      {
        ViewBag.PONumberError = "PO Number must be unique.";
        return View(viewName, addModel);
      }

      //add/update model into database
      if (ModelState.IsValid)
      {
        if (viewName != "Edit") { addModel.orderDetails = addModel.PODetails.Items; }
        await _poService.CreateOrUpdateOrderAsync(addModel);

        if (viewName == "Edit")
        {
          await _poService.DeleteOrderDetailAsync(addModel.PODetails.Items, addModel.Id);
          foreach (var i in addModel.PODetails.Items)
          {
            i.OrderId = addModel.Id;
            await _poService.CreateOrUpdateOrderDetailAsync(i);
          }
        }

        return RedirectToAction("Index", new { id = addModel.PONumber, method = viewName });
      }

      ModelState.Clear();
      return View(viewName, addModel);
    }

    [HttpPost]
    public async Task<ActionResult> AddItem(string method, List<string> orderDetails, List<string> newItem, int? currentPage)
    {
      OrderDTO addModel = new OrderDTO();
      addModel.SingleOrderDetail = new OrderDetailDTO();
      addModel.PODetails = new PagedListResult<OrderDetailDTO>();
      addModel.PODetails.Items = new List<OrderDetailDTO>();

      SetDropDownList();
      ViewBag.CurrentPage = currentPage ?? 1;
      ViewBag.PageSize = pageSize;
      if (newItem.Count > 0)
      {
        addModel.SingleOrderDetail = ConvertToDto(newItem)[0];
      }
      if (orderDetails.Count > 0)
      {
        addModel.PODetails.Items = ConvertToDto(orderDetails);
      }

      switch (method)
      {
        case "Previous":
          ViewBag.CurrentPage--;
          break;
        case "Next":
          ViewBag.CurrentPage++;
          break;
        case "Save":
          if (addModel.PODetails == null) { addModel.PODetails = new PagedListResult<OrderDetailDTO>(); }
          if (addModel.PODetails.Items == null) { addModel.PODetails.Items = new List<OrderDetailDTO>(); }

          if ((!UniqueItemNumber(-1, addModel.SingleOrderDetail.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.SingleOrderDetail.ItemNumber, addModel.SingleOrderDetail.Id))))
          {
            ViewBag.ItemNumberError = "Item Number must be unique.";
            ViewBag.ItemId = -1;
            return PartialView("_OrderDetail", addModel);
          }

          addModel.PODetails.Items.Add(addModel.SingleOrderDetail);
          ViewBag.ItemId = -2;
          break;
        case "AddItem":
          ViewBag.ItemId = -1;
          break;
        default:
          int itemId = int.Parse(new string(method.Where(char.IsDigit).ToArray()));
          if (method.Contains("Update"))
          {
            if ((!UniqueItemNumber(itemId, addModel.SingleOrderDetail.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.SingleOrderDetail.ItemNumber, addModel.SingleOrderDetail.Id))))
            {
              ViewBag.ItemNumberError = "Item Number must be unique.";
              ViewBag.ItemId = itemId;
              return PartialView("_OrderDetail", addModel);
            }

            addModel.PODetails.Items[itemId] = addModel.SingleOrderDetail;
            ViewBag.ItemId = -2;
          }
          else if (method.Contains("Delete"))
          {
            addModel.PODetails.Items.Remove(addModel.PODetails.Items[itemId]);
            ViewBag.CurrentPage = 1;
          }
          else if (method.Contains("Page"))
          {
            ViewBag.CurrentPage = itemId;
          }
          else
          {
            addModel.SingleOrderDetail = addModel.PODetails.Items[itemId];
            ViewBag.ItemId = itemId;
          }
          break;
      }

      ModelState.Clear();
      return PartialView("_OrderDetail", addModel);
    }


    public async Task<ActionResult> Edit(string PONumber)
    {
      if (PONumber == null)
      {
        return StatusCode(404);
      }

      OrderDTO editModel = new OrderDTO();
      editModel = await _poService.GetOrderByPONumber(PONumber);

      if (editModel.Id == 0)
      {
        return StatusCode(404);
      }

      editModel.Method = "Edit Order Number: " + editModel.PONumber;
      editModel.SingleOrderDetail = new OrderDetailDTO();
      editModel.SingleOrderDetail.OrderId = editModel.Id;
      editModel.SingleOrderDetail.ItemNumber = editModel.PODetails.Items[0].ItemNumber;

      SetDropDownList();
      ViewBag.ItemId = -2;
      ViewBag.PageSize = pageSize;

      return View(editModel);
    }

    //Copy an Order
    public async Task<ActionResult> Copy(string PONumber)
    {
      if (PONumber == null)
      {
        return StatusCode(404);
      }

      ModelState.Clear();
      OrderDTO copyModel = new OrderDTO();
      copyModel = await _poService.GetOrderByPONumber(PONumber);

      if (copyModel.Id == 0)
      {
        return StatusCode(404);
      }

      copyModel.Id = 0;

      copyModel.Method = "Copy from Order Number: " + copyModel.PONumber;
      copyModel.OrderDate = DateTime.Now;
      copyModel.ShipDate = DateTime.Now;
      copyModel.LatestShipDate = DateTime.Now;
      copyModel.DeliveryDate = DateTime.Now;
      copyModel.PODetails = new PagedListResult<OrderDetailDTO>();
      copyModel.PODetails.Items = new List<OrderDetailDTO>();
      copyModel.SingleOrderDetail = new OrderDetailDTO();
      SetDropDownList();

      copyModel.PONumber = "";
      ViewBag.ItemId = -2;
      ViewBag.PageSize = pageSize;

      return View(copyModel);
    }

    //convert item properties from string type to correct type in DTO
    private List<OrderDetailDTO> ConvertToDto(List<string> orderDetailStrings)
    {
      List<OrderDetailDTO> orderDetailDTOs = new List<OrderDetailDTO>();

      int totalProperty = 16;
      for (int i = 0; i < (orderDetailStrings.Count / totalProperty); i++)
      {
        OrderDetailDTO orderDetail = new OrderDetailDTO();

        orderDetail.Id = int.Parse(orderDetailStrings[0 + (i * totalProperty)]);
        orderDetail.OrderId = int.Parse(orderDetailStrings[1 + (i * totalProperty)]);
        orderDetail.RowVersion = orderDetailStrings[2 + (i * totalProperty)];

        orderDetail.ItemNumber = orderDetailStrings[3 + (i * totalProperty)];
        orderDetail.Description = orderDetailStrings[4 + (i * totalProperty)];
        orderDetail.Tariff = orderDetailStrings[5 + (i * totalProperty)];
        orderDetail.Quantity = decimal.Parse(orderDetailStrings[6 + (i * totalProperty)]);
        orderDetail.Cartons = float.Parse(orderDetailStrings[7 + (i * totalProperty)]);
        orderDetail.Cube = float.Parse(orderDetailStrings[8 + (i * totalProperty)]);
        orderDetail.KGS = float.Parse(orderDetailStrings[9 + (i * totalProperty)]);
        orderDetail.UnitPrice = float.Parse(orderDetailStrings[10 + (i * totalProperty)]);
        orderDetail.RetailPrice = float.Parse(orderDetailStrings[11 + (i * totalProperty)]);
        orderDetail.Warehouse = orderDetailStrings[12 + (i * totalProperty)];
        orderDetail.Size = orderDetailStrings[13 + (i * totalProperty)];
        orderDetail.Colour = orderDetailStrings[14 + (i * totalProperty)];
        orderDetail.Status = OrderStatus.AwaitingBooking;

        orderDetailDTOs.Add(orderDetail);
      }

      return orderDetailDTOs;
    }

    //check if ItemNumber is unique within list of items on view ONLY, NOT compare with database
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
        for (var i = 0; i < orderDetails.Count(); i++)
        {
          if ((itemNum == orderDetails[i].ItemNumber) && (id != i)) { return false; }
        }
      }

      return true;
    }

    //Set DropDownList to select on View
    private void SetDropDownList()
    {
      ViewBag.Seasons = SeasonList();
      ViewBag.Ports = new List<string> { "Cẩm Phả", "Hòn Gai", "Hải Phòng", "Nghi Sơn", "Cửa Lò" };
      ViewBag.Modes = new List<string> { "Road", "Sea", "Air" };
      ViewBag.Origins = new List<string> { "HongKong", "Vietnam" };

      List<string> temp = new List<string>();

      foreach (var i in Enum.GetValues(typeof(Currency)))
      {
        temp.Add(i.ToString());
      }
      ViewBag.Currencies = temp;

      temp = new List<string>();

      foreach (var i in Enum.GetValues(typeof(OrderStatus)))
      {
        temp.Add(i.ToString());
      }
      ViewBag.Statuses = temp;
    }

    //Set list of years for Season field
    private List<string> SeasonList()
    {
      List<string> seasonList = new List<string>();
      for (int i = 2010; i <= 2020; i++)
      {
        seasonList.Add(i.ToString());
      }
      return seasonList;
    }
  }
}