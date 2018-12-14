using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.ShipmentTrack
{
    public class CA : EntityBase
    {
        public int BookingId { get; set; }

        public DateTime ArrivalDate { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
