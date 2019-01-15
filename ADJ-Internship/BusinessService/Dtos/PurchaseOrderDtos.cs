using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ADJ.BusinessService.Core;
using ADJ.BusinessService.Validators;
using ADJ.Common;
using ADJ.DataModel;
using ADJ.DataModel.Core;
using ADJ.DataModel.OrderTrack;
using AutoMapper;

namespace ADJ.BusinessService.Dtos
{
  public class PurchaseOrderDto : EntityDtoBase, ICreateMapping
  {
    public string Test { get; set; }

    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<PurchaseOrder, PurchaseOrderDto>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<PurchaseOrderDto, PurchaseOrder>().IncludeBase<EntityDtoBase, EntityBase>();
    }
  }

  public class CreateOrUpdatePurchaseOrderRq : EntityDtoBase, ICreateMapping
  {
    public string Test { get; set; }

    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<CreateOrUpdatePurchaseOrderRq, PurchaseOrder>().IncludeBase<EntityDtoBase, EntityBase>();
    }
  }

  public class OrderDTO : EntityDtoBase, ICreateMapping
  {
    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<Order, OrderDTO>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<OrderDTO, Order>().IncludeBase<EntityDtoBase, EntityBase>();
    }

    //Default Date = Today's Date
    static DateTime DefaultDate = DateTime.Now;

    [Required]
    [Display(Name = "PO Number")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Numbers only")]
    public string PONumber { get; set; }

    //DropList from 2010 to 2020
    //Default value is Current Date
    [Display(Name = "Order Date")]
    [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    public DateTime OrderDate { get; set; } = DefaultDate;

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Buyer { get; set; }

    //Default value = USD
    public Currency Currency { get; set; }

    //Droplist from 2010 to 2020
    public string Season { get; set; }

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Department { get; set; }

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Vendor { get; set; }

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Company { get; set; }

    //Droplist Vietnam-HongKong
    [Required]
    public string Origin { get; set; }

    //Droplist of Ports
    [Required]
    [Display(Name = "Port of Loading")]
    [PortIsDifferent("PortOfDelivery")]
    public string PortOfLoading { get; set; }

    [Required]
    [Display(Name = "Port of Delivery")]
    [PortIsDifferent("PortOfLoading")]
    public string PortOfDelivery { get; set; }

    [Display(Name = "Order Type")]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string OrderType { get; set; }

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Factory { get; set; }

    //DropList Road-Sea-Air
    [Display(Name = "Ship Method")]
    public string Mode { get; set; }

    [Display(Name = "Ship Date")]
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    public DateTime ShipDate { get; set; } = DefaultDate;

    [Display(Name = "Latest Ship Date")]
    [Not30DaysApart("ShipDate")]
    [SimilarOrLaterThanOtherDate("ShipDate")]
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    public DateTime LatestShipDate { get; set; } = DefaultDate;

    [Display(Name = "Delivery Date")]
    [Not30DaysApart("ShipDate")]
    [TwoDaysLaterThanOtherDate("ShipDate")]
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    public DateTime DeliveryDate { get; set; } = DefaultDate;

    //sum of all PODetails Quantity 
    public decimal POQuantity { get; set; }

    //Default value = "New"
    public OrderStatus Status { get; set; }

    public virtual List<OrderDetailDTO> orderDetails { get; set; }


    public virtual PagedListResult<OrderDetailDTO> PODetails { get; set; }

    public OrderDetailDTO SingleOrderDetail { get; set; }

    public string Method { get; set; }
  }

  public class OrderDetailDTO : EntityDtoBase, ICreateMapping
  {
    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<OrderDetail, OrderDetailDTO>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<OrderDetailDTO, OrderDetail>().IncludeBase<EntityDtoBase, EntityBase>();
    }

    [Required]
    [Display(Name = "Item Number")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Numbers only")]
    public string ItemNumber { get; set; }

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Description { get; set; } = "";

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Warehouse { get; set; } = "";

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Colour { get; set; } = "";

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Size { get; set; } = "";

    [Required]
    [Display(Name = "Item Quantity")]
    //[Range(0, 9999999999, ErrorMessage = "Must be between 0 and 9999999999")]
    //[RegularExpression(@"^\d+$", ErrorMessage = "Must be an integer")]
    public decimal Quantity { get; set; }

    public decimal ReviseQuantity { get; set; }

    [Required]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    [RegularExpression(@"^[-+]?\d+(\.[0]+)?$", ErrorMessage = "Must be an integer")]
    public float Cartons { get; set; }

    [Required]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    [RegularExpression(@"^[-+]?\d+(\.[0]+)?$", ErrorMessage = "Must be an integer")]
    public float Cube { get; set; }

    [Required]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float KGS { get; set; }

    [Required]
    [Display(Name = "Unit Price")]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float UnitPrice { get; set; }

    //Item Quantity*Unit Price = Total Price
    [Display(Name = "Total Price")]
    public float TotalPrice
    {
      get
      {
        return (float)Quantity * UnitPrice;
      }
    }

    [Required]
    [Display(Name = "Retail Price")]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float RetailPrice { get; set; }

    //Item Quantity*Retail Price = Total Retail Price
    [Display(Name = "Total Retail Price")]
    public float TotalRetailPrice
    {
      get
      {
        return (float)Quantity * RetailPrice;
      }
    }

    [Display(Name = "Tariff Code")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Tariff must be numeric")]
    public string Tariff { get; set; } = "";

    public OrderStatus Status { get; set; }

    [Required]
    //[ForeignKey("OrderModel")]
    public int OrderId { get; set; }
  }
}
