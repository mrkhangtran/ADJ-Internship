using ADJ.Common;
using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataModel.ShipmentTrack
{
  public class Booking : EntityBase
  {
    public Guid ShipmentID { get; set; }

    public string PONumber { get; set; }

    public string Line { get; set; }

    public string ItemNumber { get; set; }

    public string Factory { get; set; }

    public string Carrier { get; set; }

    public string Vessel { get; set; }

    public DateTime ETD { get; set; }

    public DateTime ETA { get; set; }

    public string Voyage { get; set; }

    public decimal Quantity { get; set; }

    public float Cartons { get; set; }

    public float Cube { get; set; }

    public string PackType { get; set; }

    public string PortOfLoading { get; set; }

    public string PortOfDelivery { get; set; }

    public string LoadingType { get; set; }

    public string Mode { get; set; }

    public string FreightTerms { get; set; }

    public string Consignee { get; set; }

    public decimal GrossWeight { get; set; }

    public DateTime BookingDate { get; set; }

    public string BookingType { get; set; }

    public OrderStatus Status { get; set; }

    public virtual ICollection<Manifest> Manifests { get; set; }

    public virtual ICollection<ArriveOfDespatch> ArriveOfDespatches { get; set; }

    public virtual ICollection<CA> CAs { get; set; }
  }
}
