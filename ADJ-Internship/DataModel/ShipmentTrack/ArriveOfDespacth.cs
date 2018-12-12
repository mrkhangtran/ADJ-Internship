using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataModel.ShipmentTrack
{
    public class ArriveOfDespacth : EntityBase
    {
        //[ForeignKey("BookingModel")]
        public int BookingId { get; set; }
        public string Carrier { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CTD { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ETA { get; set; }
        public string OriginPort { get; set; }
        public string DestinationPort { get; set; }
        public string Mode { get; set; }
        public string Confirmed { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
