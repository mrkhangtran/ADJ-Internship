using ADJ.BusinessService.Core;
using ADJ.DataModel.Core;
using ADJ.DataModel.DeliveryTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
  public class DCBookingDtos : EntityDtoBase, ICreateMapping
  {
    public string Name { get; set; }

    public int ContainerId { get; set; }

    [DisplayName("Dest Port")]
    public string DestPort {get;set;}

    public string ArrivalDate { get; set; }

    public string DistributionCenter { get; set; }

    public string WareHouse { get; set; }

    public string BookingRef { get; set; }

    public DateTime BookingDate { get; set; }

    public string BookingTime { get; set; }

    public string Haulier { get; set; }

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
}
