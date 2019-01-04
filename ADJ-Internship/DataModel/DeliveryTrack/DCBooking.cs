using ADJ.DataModel.Core;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.DeliveryTrack
{
	public class DCBooking : EntityBase
	{
		public int ContainerId { get; set; }

		public string DítributionCenter { get; set; }

		public string WareHouse { get; set; }

		public string BookingRef { get; set; }

		public DateTime BookingDate { get; set; }

		public string BookingTime { get; set; }

		public string Haulier { get; set; }

		public string Client { get; set; }

		public DateTime Created { get; set; }

		public virtual Container Container { get; set; }

		public virtual ICollection<DCBookingDetail> DCBookingDetails { get; set; }

	}
}
