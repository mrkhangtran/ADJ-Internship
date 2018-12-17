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
      defaultModel.PODetails = new PagedListResult<OrderDetailDTO>();
      defaultModel.PODetails.PageCount = 1;
      defaultModel.PODetails.TotalCount = 2;
      SetDropDownList();
      ViewBag.ItemId = -2;

      return View(defaultModel);
    }

    [HttpPost]
    public async Task<ActionResult> Create(OrderDTO addModel, string method)
    {
      SetDropDownList();

      switch (method)
      {
        case "Apply":
          if (addModel.PODetails.Items == null)
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
            addModel.orderDetails = addModel.PODetails.Items;
            await _poService.CreateOrUpdateOrderAsync(addModel);

            return RedirectToAction("Index", new { id = addModel.PONumber, method = 1 });
          }
          break;
        case "Save":
          if (addModel.PODetails == null) { addModel.PODetails = new PagedListResult<OrderDetailDTO>(); }
          if (addModel.PODetails.Items == null) { addModel.PODetails.Items = new List<OrderDetailDTO>(); }

          if ((!UniqueItemNumber(-1, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
          {
            ViewBag.ItemNumberError = "Item Number must be unique.";
            ViewBag.ItemId = -1;
            return View(addModel);
          }

          addModel.PODetails.Items.Add(addModel.orderDetailDTO);
          addModel.PODetails.PageCount = 1;
          ViewBag.ItemId = -2;
          break;
        case "AddItem":
          ViewBag.ItemId = -1;
          return PartialView("_OrderDetail",addModel);
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
            addModel.PODetails.PageCount = 1;
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
      editModel.PODetails.TotalCount = 2;

      ViewBag.Method = "Edit Order Number: " + editModel.PONumber;
      SetDropDownList();
      ViewBag.ItemId = -2;

      return View(editModel);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(OrderDTO addModel, string method)
    {
      ViewBag.Method = "Edit Order Number: " + addModel.PONumber;
      SetDropDownList();

      switch (method)
      {
        case "Apply":
          if (addModel.PODetails.Items == null)
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
            await _poService.DeleteOrderDetailAsync(addModel.PODetails.Items, addModel.Id);
            foreach (var i in addModel.PODetails.Items)
            {
              i.OrderId = addModel.Id;
              await _poService.CreateOrUpdateOrderDetailAsync(i);
            }

            return RedirectToAction("Index", new { id = addModel.PONumber, method = 2 });
          }
          break;
        case "Save":
          if (addModel.PODetails == null) { addModel.PODetails = new PagedListResult<OrderDetailDTO>(); }
          if (addModel.PODetails.Items == null) { addModel.PODetails.Items = new List<OrderDetailDTO>(); }

          if ((!UniqueItemNumber(-1, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
          {
            ViewBag.ItemNumberError = "Item Number must be unique.";
            ViewBag.ItemId = -1;
            return View(addModel);
          }

          addModel.PODetails.Items.Add(addModel.orderDetailDTO);
          addModel.PODetails.PageCount = 1;
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
            addModel.PODetails.PageCount = 1;
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

    public async Task<ActionResult> Copy(string PONumber)
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
      copyModel.PODetails.TotalCount = 2;
      copyModel.orderDetailDTO = new OrderDetailDTO();
      SetDropDownList();

      ViewBag.Method = "Copy from Order Number: " + copyModel.PONumber;
      copyModel.PONumber = "";
      ViewBag.ItemId = -2;

      return View(copyModel);
    }

    [HttpPost]
    public async Task<ActionResult> Copy(OrderDTO addModel, string method)
    {
      SetDropDownList();

      switch (method)
      {
        case "Previous":
          addModel.PODetails.PageCount++;
          break;
        case "Next":
          addModel.PODetails.PageCount--;
          break;
        case "Apply":
          if (addModel.PODetails.Items == null)
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
            addModel.orderDetails = addModel.PODetails.Items;
            await _poService.CreateOrUpdateOrderAsync(addModel);

            return RedirectToAction("Index", new { id = addModel.PONumber, method = 3 });
          }
          break;
        case "Save":
          if (addModel.PODetails == null) { addModel.PODetails = new PagedListResult<OrderDetailDTO>(); }
          if (addModel.PODetails.Items == null) { addModel.PODetails.Items = new List<OrderDetailDTO>(); }

          if ((!UniqueItemNumber(-1, addModel.orderDetailDTO.ItemNumber, addModel.PODetails.Items)) || (!(await _poService.UniqueItemNumAsync(addModel.orderDetailDTO.ItemNumber, addModel.orderDetailDTO.Id))))
          {
            ViewBag.ItemNumberError = "Item Number must be unique.";
            ViewBag.ItemId = -1;
            return View(addModel);
          }

          addModel.PODetails.Items.Add(addModel.orderDetailDTO);
          addModel.PODetails.PageCount = 1;
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
            addModel.PODetails.PageCount = 1;
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