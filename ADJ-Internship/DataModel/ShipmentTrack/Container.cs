using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.DataModel.DeliveryTrack;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataModel.ShipmentTrack
{
  public class Container : EntityBase
  {
    public string Name { get; set; }

    public string Size { get; set; }

    public string Loading { get; set; }

    public string PackType { get; set; }

    public ContainerStatus Status { get; set; }

    public virtual CA CA { get; set; }

    public virtual DCBooking DCBooking { get; set; }

    public virtual DCConfirmation DCConfirmation { get; set; }

    public virtual ArriveOfDespatch ArriveOfDespatch { get; set; }

    public virtual ICollection<Manifest> Manifests { get; set; }
  }
}
