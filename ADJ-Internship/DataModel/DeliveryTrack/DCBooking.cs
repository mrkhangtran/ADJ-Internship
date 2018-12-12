using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.DeliveryTrack
{
    public class DCBooking : EntityBase
    {

        [StringLength(20)]
        public string DeliveryMethod { get; set; }

        [StringLength(50)]
        public string WareHouse { get; set; }

        [StringLength(12)]
        public string BookingRef { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BookingDate { get; set; }

        [StringLength(12)]
        public string BookingTime { get; set; }

        [StringLength(30)]
        public string Haulier { get; set; }


        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Created { get; set; }

        public virtual ICollection<DCBookingDetail> DCBookingDetails { get; set; }

    }
}
