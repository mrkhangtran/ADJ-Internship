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
  public class VesselDepartureDtos
  {
    public FilterDto FilterDto { get; set; }

    public List<ContainerInfoDto> ContainerInfoDtos { get; set; }

    public PagedListResult<ContainerDto> ResultDtos { get; set; }
  }

  public class FilterDto
  {
    //Droplist Vietnam-HongKong
    public string Origin { get; set; }

    //Droplist of Ports
    [Display(Name = "Origin Port")]
    public string OriginPort { get; set; }

    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Letters and numbers only")]
    public string Container { get; set; }

    //Droplist of Pending-Despatched
    public string Status { get; set; }

    [Display(Name = "ETD From")]
    public DateTime? ETDFrom { get; set; }

    [Display(Name = "ETD To")]
    [SimilarOrLaterThanOtherDate("ETDFrom")]
    public DateTime? ETDTo { get; set; }
  }

  public class ContainerInfoDto
  {
    //Droplist of Ports
    [Display(Name = "Origin Port")]
    [PortIsDifferent("DestinationPort")]
    public string OriginPort { get; set; }

    //Droplist of Ports
    [Display(Name = "Destination Port")]
    [PortIsDifferent("OriginPort")]
    public string DestinationPort { get; set; }

    //DropList of Road-Sea-Air
    public string Mode { get; set; }

    //Droplist of Carrier
    public string Carrier { get; set; }
  }

  public class ContainerDto : EntityDtoBase, ICreateMapping
  {
    [Display(Name = "Origin Port")]
    public string OriginPort { get; set; }

    [Display(Name = "Destination Port")]
    public string DestinationPort { get; set; }

    public string Mode { get; set; }

    public string Carrier { get; set; }
    
    public DateTime ETD { get; set; }

    public DateTime ETA { get; set; }

    public int ContainerId { get; set; }

    public int BookingId { get; set; }

    public bool Selected { get; set; }

    [Display(Name = "Container")]
    public string Name { get; set; }

    public string Size { get; set; }

    public int GroupId { get; set; }

    public ContainerStatus Status { get; set; }

    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<Booking, ContainerDto>().IncludeBase<EntityBase, EntityDtoBase>();

      profile.CreateMap<ArriveOfDespatch, ContainerDto>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<ContainerDto, ArriveOfDespatch>().IncludeBase<EntityDtoBase, EntityBase>();

      profile.CreateMap<Container, ContainerDto>().IncludeBase<EntityBase, EntityDtoBase>();
    }
  }
}
