using ADJ.BusinessService.Core;
using ADJ.BusinessService.Validators;
using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.DataModel.ShipmentTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
  public class ConfirmArrivalDtos : EntityDtoBase, ICreateMapping
  {
    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<CA, ConfirmArrivalDtos>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<ConfirmArrivalDtos, CA>().IncludeBase<EntityDtoBase, EntityBase>();
    }

    public int ContainerId { get; set; }

    [Display(Name = "Arrival Date")]
    public DateTime ArrivalDate { get; set; }

    public ConfirmArrivalFilterDtos FilterDtos { get; set; }

    public List<ConfirmArrivalResultDtos> Containers { get; set; }
  }

  public class ConfirmArrivalFilterDtos : EntityDtoBase
  {
    //Droplist HongKong-Vietnam
    public string Origin { get; set; }

    //DropList Road-Sea-Air
    public string Mode { get; set; }

    [StringLength(30)]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string Vendor { get; set; } = "";

    [StringLength(10)]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
    public string Container { get; set; } = "";

    [Display(Name = "ETA From")]
    public DateTime? ETAFrom { get; set; }

    [Display(Name = "ETA To")]
    [SimilarOrLaterThanOtherDate("ETAFrom")]
    public DateTime? ETATo { get; set; }

    //DropList of Despatched and Arrived
    public string Status { get; set; }
  }

  public class ConfirmArrivalResultDtos : EntityDtoBase
  {
    [Display(Name = "Destination Port")]
    public string DestinationPort { get; set; }

    public string Origin { get; set; }

    public string Mode { get; set; }

    public string Carrier { get; set; }

    [Display(Name = "Destination Port")]
    public DateTime ArrivalDate { get; set; }

    public bool Selected { get; set; }

    public string Vendor { get; set; }

    public string Container { get; set; }

    public ContainerStatus Status { get; set; }
  }
}
