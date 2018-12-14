using ADJ.DataModel.Core;
using ADJ.DataModel.OrderTrack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.DeliveryTrack
{
    public class DCBookingDetail : EntityBase
    {
        [StringLength(30)]
        public string Line { get; set; }

        public decimal Quantity { get; set; }

        public int Cartons { get; set; }

        public decimal Cube { get; set; }

        [StringLength(50)]
        public string Item { get; set; }

        public int OrderId { get; set; }

        [StringLength(20)]
        public string Container { get; set; }

        public int DCBookingId { get; set; }

        public virtual Order Order { get; set; }

        public virtual DCBooking DCBooking { get; set; }
    }
}
