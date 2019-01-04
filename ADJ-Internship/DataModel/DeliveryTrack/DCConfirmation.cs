using ADJ.DataModel.Core;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.DeliveryTrack
{
	public class DCConfirmation : EntityBase
	{
		public int ContainerId { get; set; }

		public DateTime DeliveryDate { get; set; }

		public string DeliveryTime { get; set; }

		public virtual Container Container { get; set; }
	}
}
