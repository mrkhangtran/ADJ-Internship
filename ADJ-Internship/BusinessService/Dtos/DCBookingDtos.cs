using ADJ.BusinessService.Core;
using ADJ.BusinessService.Validators;
using ADJ.DataModel.Core;
using ADJ.DataModel.DeliveryTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
  public class DCBookingDtos : EntityDtoBase, ICreateMapping
  {
    public string Name { get; set; }

    public int ContainerId { get; set; }

    [DisplayName("Dest Port")]
    public string DestPort {get;set;}

    public DateTime ArrivalDate { get; set; }

    public string DistributionCenter { get; set; }

    public string WareHouse { get; set; }
    public string BookingRef { get; set; }
    static DateTime defaultDate = DateTime.Now;
    [DCBookingDtoValidators("ArrivalDate")]
    [Required]
    public DateTime BookingDate { get; set; } = defaultDate;

    

    [RegularExpression("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time is invalid")]
    [Required]
    public string BookingTime { get; set; } = defaultDate.ToString("hh:mm");

    public string Haulier { get; set; }
    [StringLength(20)]
    public string Client { get; set; }

    public DateTime Created { get; set; }

    public virtual Container Container { get; set; }

    public virtual ICollection<DCBookingDetail> DCBookingDetails { get; set; }

    public decimal ShipCarton { get; set; }

    public  decimal ShipCube { get; set; }

    public decimal ShipQuantity { get; set; }

    public string Status { get; set; }

    public bool selected { get; set; }

    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<DCBooking, DCBookingDtos>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<DCBookingDtos, DCBooking>().IncludeBase<EntityDtoBase, EntityBase>();
    }
  }
  public class SearchingDCBooking
  {
    public IEnumerable<string> DestinationPort { get; set; }
    public IEnumerable<string> Status { get; set; }
  }
}
