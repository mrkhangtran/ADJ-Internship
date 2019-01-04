using ADJ.BusinessService.Core;
using ADJ.BusinessService.Validators;
using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.DataModel.DeliveryTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
  public class DCReceivedDtos : EntityDtoBase, ICreateMapping
  {
    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<DCConfirmation, DCReceivedDtos>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<DCReceivedDtos, DCConfirmation>().IncludeBase<EntityDtoBase, EntityBase>();
    }

    public int ContainerId { get; set; }

    [Display(Name = "Delivery Date")]
    public DateTime DeliveryDate { get; set; }

    [Display(Name = "Delivery Time")]
    public string DeliveryTime { get; set; }

    DCReceivedFilterDtos FilterDtos { get; set; }
  }

  public class DCReceivedFilterDtos
  {
    [StringLength(10)]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Container { get; set; }

    //DropList of DCs
    public string DC { get; set; }

    [Display(Name = "Booking Date From")]
    public DateTime? BookingDateFrom { get; set; }

    [Display(Name = "Booking Date To")]
    [SimilarOrLaterThanOtherDate("BookingDateFrom")]
    public DateTime? BookingDateTo { get; set; }

    [StringLength(10)]
    [Display(Name = "Booking Ref")]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string BookingRef { get; set; }

    [Display(Name = "Delivery Date From")]
    public DateTime? DeliveryDateFrom { get; set; }

    [Display(Name = "Delivery Date To")]
    [SimilarOrLaterThanOtherDate("DeliveryDateFrom")]
    public DateTime? DeliveryDateTo { get; set; }

    //Droplist of [DC Booking Received] and [ Delivered]
    public ContainerStatus Status { get; set; }

    [StringLength(10)]
    [Display(Name = "Item Number")]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string ItemNumber { get; set; }
  }

  public class DCReceivedResultDtos
  {
    public string Container { get; set; }
    
    public string DC { get; set; }

    public string Haulier { get; set; }

    public DateTime BookingDate { get; set; }

    public string BookingTime { get; set; }

    public string BookingRef { get; set; }

    public DateTime DeliverDate { get; set; }

    public string DeliverTime { get; set; }

    public ContainerStatus Status { get; set; }
  }
}
