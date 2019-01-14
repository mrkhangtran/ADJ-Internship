using ADJ.BusinessService.Core;
using ADJ.BusinessService.Validators;
using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.DataModel.OrderTrack;
using ADJ.DataModel.ShipmentTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
  public class ShipmentBookingDtos : EntityDtoBase, ICreateMapping
  {
    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<Booking, ShipmentBookingDtos>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<ShipmentBookingDtos, Booking>().IncludeBase<EntityDtoBase, EntityBase>();
    }

    static DateTime defaultDate = DateTime.Now;

    //Droplist of Ports, alphabetical ascending order
    [Required]
    [Display(Name = "Origin Port")]
    [PortIsDifferent("PortOfDelivery")]
    public string PortOfLoading { get; set; }

    //Droplist of Carriers, alphabetical ascending order
    [Required]
    public string Carrier { get; set; }

    //Droplist of Ports, alphabetical ascending order
    [Required]
    [Display(Name = "Destination Port")]
    [PortIsDifferent("PortOfLoading")]
    public string PortOfDelivery { get; set; }

    //DropList Road-Sea-Air
    [Required]
    public string Mode { get; set; }

    [Required]
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    //[LaterThanOtherDate("POShipDate")]
    public DateTime ETD { get; set; } = defaultDate;

    [Required]
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    [LaterThanOtherDate("ETD")]
    [Not30DaysApart("ETD")]
    public DateTime ETA { get; set; } = defaultDate;

    public int OrderId { get; set; }

    public string PONumber { get; set; }

    public string ItemNumber { get; set; }

    //DropList of Boxed/Carton
    public string PackType { get; set; }

    public decimal Quantity { get; set; }

    public float Cartons { get; set; }

    public float Cube { get; set; }

    public OrderStatus Status { get; set; }

    public Guid ShipmentID { get; set; }

    public List<ShipmentResultDtos> OrderDetails { get; set; }

    public ShipmentFilterDtos FilterDtos { get; set; }
  }

  public class ShipmentFilterDtos
  {
    //Droplist Vietnam-HongKong
    public string Origin { get; set; }

    //Droplist of Ports, alphabetical ascending order
    [Display(Name = "Origin Port")]
    public string OriginPort { get; set; }

    //DropList Road-Sea-Air
    public string Mode { get; set; }

    [StringLength(30)]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Warehouse { get; set; } = "";

    //DropList of Awaiting_Booking and Booking_Made
    public string Status { get; set; }

    [StringLength(30)]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Vendor { get; set; } = "";

    [Display(Name = "PO Number")]
    [StringLength(10, ErrorMessage = "Cannot be longer than 10 characters")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Numbers only")]
    public string PONumber { get; set; }

    [Display(Name = "Item Number")]
    [StringLength(10, ErrorMessage = "Cannot be longer than 10 characters")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Numbers only")]
    public string ItemNumber { get; set; }

    public List<OrderDetailDTO> OrderDetails { get; set; }
  }

  public class ShipmentResultDtos : EntityDtoBase
  {
    public bool Selected { get; set; }

    [Display(Name = "PO Number")]
    public string PONumber { get; set; }

    [Display(Name = "Item Number")]
    public string ItemNumber { get; set; }

    public string Vendor { get; set; }

    [Display(Name = "PO Quantity")]
    public decimal Quantity { get; set; }

    //equal Revise Quantity
    [Display(Name = "Booking Quantity")]
    public decimal BookingQuantity { get; set; }

    public float Cartons { get; set; }

    public float Cube { get; set; }

    [Display(Name = "Booking Cartons")]
    public decimal BookingCartons
    {
      get
      {
        return BookingQuantity * (decimal)Cartons;
      }
    }

    [Display(Name = "Booking Cube")]
    public decimal BookingCube
    {
      get
      {
        return BookingQuantity * (decimal)Cube;
      }
    }

    //DropList of Boxed/Carton
    public string PackType { get; set; }

    [Display(Name = "PO Ship Date")]
    public DateTime POShipDate { get; set; }

    public DateTime DeliveryDate { get; set; }

    //Default as "Awaiting Booking"
    [Display(Name = "Booking Status")]
    public OrderStatus Status { get; set; }

    public string StatusDescription { get; set; }

    public int OrderId { get; set; }
  }

  public class DropDownList
  {
    List<string> Ports { get; set; }
  }
}
