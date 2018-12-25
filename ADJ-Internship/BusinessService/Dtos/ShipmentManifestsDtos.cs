using ADJ.BusinessService.Core;
using ADJ.DataModel.Core;
using ADJ.DataModel.ShipmentTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
  public class ShipmentManifestsDtos : EntityDtoBase, ICreateMapping
  {
    public string Container { get; set; } 

    public string Size { get; set; }

    public string Loading { get; set; }

    public string PackType { get; set; }

    public List<ItemManifest> itemManifests { get; set; }



    public void CreateMapping(Profile profile)
    {
      profile.CreateMap<Manifest, ShipmentManifestsDtos>().IncludeBase<EntityBase, EntityDtoBase>();
      profile.CreateMap<ShipmentManifestsDtos, Manifest>().IncludeBase<EntityDtoBase, EntityBase>();
    }

  }
  public class ItemManifest
  {
    public bool selected { get; set; }

    public string Supplier { get; set; }

    public string Carrier { get; set; }

    public string PONumber { get; set; }

    public string ItemNumber { get; set; }

    public decimal BookingQuantity { get; set; }

    public decimal OpenQuantity { get; set; }     //= Booking Quantity - Total Ship Quantity (recorded in database)

    public decimal ShipQuantity { get; set; }

    public decimal BookingCartons { get; set; } //= Booking Qty* Carton(of 1 item)

    public decimal ShipCartons { get; set; } // Ship Qty*Carton(of 1 item)

    public decimal BookingCube { get; set; } //= Booking Qty * Cube(of 1 item)

    public decimal ShipCube { get; set; } //=Ship Qty*Cube(of 1 item)

    public decimal NetWeight { get; set; } //= Ship Qty*KGS(of 1 item)

    public DateTime ETDDate { get; set; } //value taken from shipment booking

    public decimal GrossWeight { get; set; }

    public string Manifested { get; set; }
  }
}
