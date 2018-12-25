using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataModel.ShipmentTrack
{
  public class Manifest : EntityBase
  {
    public int BookingId { get; set; }

    public string Seal { get; set; }

    public string Loading { get; set; }

    public int Bars { get; set; }

    public string Equipment { get; set; }

    public decimal Quantity { get; set; }

    public float Cartons { get; set; }

    public float Cube { get; set; }

    public float KGS { get; set; }

    public string FreightTerms { get; set; }

    public decimal ChargeableKGS { get; set; }

    public string PackType { get; set; }

    public decimal NetKGS { get; set; }

    public string Size { get; set; }

    public int ContainerId { get; set; }

    public virtual Booking Booking { get; set; }

    public virtual Container Container { get; set; }
  }
}
