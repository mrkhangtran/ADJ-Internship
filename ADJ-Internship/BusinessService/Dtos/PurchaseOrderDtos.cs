using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ADJ.BusinessService.Core;
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
    //get data from Order model
    public class OrderDto : EntityDtoBase, ICreateMapping
    {
        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<Order, OrderDto>().IncludeBase<EntityBase, EntityDtoBase>();
            profile.CreateMap<OrderDto, Order>().IncludeBase<EntityDtoBase, EntityBase>();
        }
        public string PONumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime OrderDate { get; set; }

        public string Supplier { get; set; }

        public string Origin { get; set; }

        public string PortOfLoading { get; set; }

        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime ShipDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime DeliveryDate { get; set; }

        public string PortOfDelivery { get; set; }

    }

    //get data from OrderDetail model
    public class OrderDetailDto : EntityDtoBase, ICreateMapping
    {
        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<OrderDetail, OrderDetailDto>().IncludeBase<EntityBase, EntityDtoBase>();
            profile.CreateMap<OrderDetailDto, OrderDetail>().IncludeBase<EntityDtoBase, EntityBase>();
        }
        public decimal Quantity { get; set; }

    }
    //get data from ProgressCheck
    public class ProgressCheckDto : EntityDtoBase, ICreateMapping
    {
        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<ProgressCheck, ProgressCheckDto>().IncludeBase<EntityBase, EntityDtoBase>();
            profile.CreateMap<ProgressCheckDto, ProgressCheck>().IncludeBase<EntityDtoBase, EntityBase>();
        }
        public  bool Complete { get; set; }

    [Required]
    [Display(Name = "Item Number")]
    [StringLength(10, ErrorMessage = "Cannot be longer than 10 characters")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string ItemNumber { get; set; }

    [StringLength(255)]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string Description { get; set; } = "";

    [StringLength(30)]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string Warehouse { get; set; } = "";

    [StringLength(30)]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string Colour { get; set; } = "";

    [StringLength(30)]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string Size { get; set; } = "";

    [Required]
    [Display(Name = "Item Quantity")]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float Quantity { get; set; }

    [Required]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float Cartons { get; set; }

    [Required]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float Cube { get; set; }

    [Required]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float KGS { get; set; }

    [Required]
    [Display(Name = "Unit Price")]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float UnitPrice { get; set; }

    //Item Quantity*Unit Price = Total Price
    public float TotalPrice
    {
      get
      {
        return Quantity * UnitPrice;
      }
    }

    [Required]
    [Display(Name = "Retail Price")]
    [Range(0, float.MaxValue, ErrorMessage = "Value should not be negative")]
    public float RetailPrice { get; set; }

    //Item Quantity*Retail Price = Total Retail Price
    public float TotalRetailPrice
    {
      get
      {
        return Quantity * RetailPrice;
      }
    }

    [Display(Name = "Tariff Code")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Tariff must be numeric")]
    public string Tariff { get; set; } = "";

    [Required]
    //[ForeignKey("OrderModel")]
    public int OrderId { get; set; }
  }
}
