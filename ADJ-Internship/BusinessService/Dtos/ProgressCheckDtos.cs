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
  public class ProgressCheckDto : EntityDtoBase, ICreateMapping
  {
    [Display(Name = "PO Number")]
    public string PONumber { get; set; }
    [Display(Name = "PO Quantity")]
    public decimal POQuantity { get; set; }
    [Display(Name = "PO Check Quantity")]
    public decimal EstQtyToShip { get; set; }
    [Display(Name = "PO Ship Date")]
    public DateTime ShipDate { get; set; }
    public List<OrderDetail> ListOrderDetail { get; set; }
    public List<OrderDetailDto> ListOrderDetailDto { get; set; }
    [Display(Name = "Inspection Date")]
    [InspectionDateValidation("IntendedShipDate")]
    public DateTime InspectionDate { get; set; }
    [Display(Name = "Int Ship Date")]
    [IntShipDateLaterThanInspectionDate("InspectionDate")]
    public DateTime IntendedShipDate { get; set; }
    [Display(Name = "PO Quantity Complete ")]
    public bool Complete { get; set; }
    public string Status { get; set; }
    public string Department { get; set; }
    public bool OnSchedule { get; set; }
    public int OrderId { get; set; }
    public string Supplier { get; set; }
    public string Factory { get; set; }
    public string Origin { get; set; }
    public string OriginPort { get; set; }
    public bool selected { get; set; }
    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<ProgressCheck, ProgressCheckDto>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<ProgressCheckDto, ProgressCheck>().IncludeBase<EntityDtoBase, EntityBase>();
    }
  }
  public class GetItemSearchDto
  {
    public IEnumerable<string> Origins { get; set; }
    public IEnumerable<string> OriginPorts { get; set; }
    public IEnumerable<string> Factories { get; set; }
    public IEnumerable<string> Suppliers { get; set; }
    public IEnumerable<string> Status { get; set; }
    public IEnumerable<string> Depts { get; set; }
  }
  public class OrderDetailDto : EntityDtoBase, ICreateMapping
  {
    public string ItemNumber { get; set; }

    public string Line { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [StringLength(30)]
    public string Warehouse { get; set; }

    [StringLength(30)]
    public string Colour { get; set; }

    [StringLength(30)]
    public string Size { get; set; }

    public string Item { get; set; }

    [Required]
    public decimal Quantity { get; set; }

    [ReviseQuantityvalidate("Quantity")]
    public decimal ReviseQuantity { get; set; }

    [Required]
    public float Cartons { get; set; }

    [Required]
    public float Cube { get; set; }

    [Required]
    public float KGS { get; set; }

    [Required]
    public float UnitPrice { get; set; }

    public float TotalPrice
    {
      get
      {
        return (float)Quantity * UnitPrice;
      }
    }

    [Required]
    public float RetailPrice { get; set; }

    public float TotalRetailPrice
    {
      get
      {
        return (float)Quantity * RetailPrice;
      }
    }

    public string Tariff { get; set; }

    public bool selected { get; set; }

    [Required]
    public int OrderId { get; set; }

    public virtual Order Order { get; set; }

    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<OrderDetailDto, OrderDetail>().IncludeBase<EntityDtoBase, EntityBase>();
      profile.CreateMap<OrderDetail, OrderDetailDto>().IncludeBase<EntityBase, EntityDtoBase>();
    }
  }


  public class OrderDto : EntityDtoBase, ICreateMapping
  {

    public string PONumber { get; set; }

    public DateTime OrderDate { get; set; }

    public string Supplier { get; set; }

    public string Origin { get; set; }

    public string PortOfLoading { get; set; }

    public DateTime ShipDate { get; set; }

    public DateTime DeliveryDate { get; set; }

    public string PortOfDelivery { get; set; }

    public decimal Quantity { get; set; }

    public OrderStatus Status { get; set; }

    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<Order, OrderDto>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<OrderDto, Order>().IncludeBase<EntityDtoBase, EntityBase>();
    }

  }

}
