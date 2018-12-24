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

    //Droplist of Ports, alphabetical ascending order
    [Required]
    [Display(Name = "Origin Port")]
    public string OriginPort { get; set; }

    //Droplist of Carriers, alphabetical ascending order
    [Required]
    public string Carrier { get; set; }

    //Droplist of Ports, alphabetical ascending order
    [Required]
    [Display(Name = "Destination Port")]
    [PortIsDifferent("OriginPort")]
    public string DestinationPort { get; set; }

    //DropList Road-Sea-Air
    [Required]
    public string Mode { get; set; }

    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    public DateTime POShipDate { get; set; }

    [Required]
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    [SimilarOrLaterThanOtherDate("POShipDate")]
    public DateTime ETD { get; set; }

    [Required]
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    [SimilarOrLaterThanOtherDate("ETD")]
    public DateTime ETA { get; set; }

    public int OrderId { get; set; }

    public string Item { get; set; }

    //DropList of Boxed/Carton
    public string PackType { get; set; }

    public decimal Quantity { get; set; }

    public float Cartons { get; set; }

    public float Cube { get; set; }

    public List<ShipmentResult> OrderDetails { get; set; }

    public ShipmentFilterDtos FilterDtos { get; set; }
  }

  public class ShipmentFilterDtos : EntityDtoBase, ICreateMapping
  {
    public void CreateMapping(Profile profile)
    {
    }

    //Droplist Vietnam-HongKong
    public string Origin { get; set; }

    //Droplist of Ports, alphabetical ascending order
    [Display(Name = "Origin Port")]
    public string OriginPort { get; set; }

    //DropList Road-Sea-Air
    public string Mode { get; set; }

    [StringLength(30)]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string Warehouse { get; set; } = "";

    //DropList of Awaiting_Booking and Booking_Made
    public OrderStatus Status { get; set; }

    [StringLength(30)]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string Vendor { get; set; } = "";

    [Display(Name = "PO Number")]
    [StringLength(10, ErrorMessage = "Cannot be longer than 10 characters")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string PONumber { get; set; }

    [Display(Name = "Item Number")]
    [StringLength(10, ErrorMessage = "Cannot be longer than 10 characters")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string ItemNumber { get; set; }

    //[Required]
    //[NotInThePast(ErrorMessage = "Cannot be set in the past")]
    //[SimilarOrLaterThanOtherDate("POShipDate")]
    //public DateTime ETD { get; set; }

    public List<OrderDetailDTO> OrderDetails { get; set; }
  }

  public class ShipmentResult : EntityDtoBase, ICreateMapping
  {
    public void CreateMapping(Profile profile)
    {
    }

    [Display(Name = "PO Number")]
    public string PONumber { get; set; }

    public string ItemNumber { get; set; }

    public string Factory { get; set; }

    [Display(Name = "PO Quantity")]
    public decimal Quantity { get; set; }

    [Display(Name = "Booking Quantity")]
    public decimal BookingQuantity { get; set; }

    public float Cartons { get; set; }

    public float Cube { get; set; }

    [Display(Name = "Booking Cartons")]
    public decimal BookingCartons {
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
    [NotInThePast(ErrorMessage = "Cannot be set in the past")]
    public DateTime POShipDate { get; set; }

    //Default as "Awaiting Booking"
    [Display(Name = "Booking Status")]
    public OrderStatus Status { get; set; }
  }
}
