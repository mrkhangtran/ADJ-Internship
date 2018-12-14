using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.DeliveryTrack
{
    public class DCConfirmationDetail : EntityBase
    {
        public int DCConfirmationId { get; set; }

        [StringLength(30)]
        public string Order { get; set; }

        [StringLength(30)]
        public string Line { get; set; }

        public decimal Quantity { get; set; }

        public int Cartons { get; set; }

        [StringLength(50)]
        public string Item { get; set; }

        [StringLength(20)]
        public string Container { get; set; }

        public virtual DCConfirmation DCConfirmation { get; set; }

        public virtual ICollection<DCBookingDetail> DCBookingDetails { get; set; }
    }
}
