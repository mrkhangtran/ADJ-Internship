using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataModel.ShipmentTrack
{
    public class Booking : EntityBase
    {
        public string Order { get; set; }

        public int OrderId { get; set; }

        public string Line { get; set; }

        public string Item { get; set; }

        public string Carier { get; set; }

        public string Vessel { get; set; }

        public DateTime ETD { get; set; }

        public DateTime ETA { get; set; }

        public string Voyage { get; set; }

        public decimal Quantity { get; set; }

        public int Cartoons { get; set; }

        public decimal Cube { get; set; }

        public string PackType { get; set;}

        public string LoadingType { get; set; }

        public string Mode { get; set; }

        public string FreightTerms { get; set; }

        public string Consignee { get; set; }

        public decimal GrossWeight { get; set; }

        public DateTime BookingDate { get; set; }

        public string BookingType { get; set; }
    }
}
