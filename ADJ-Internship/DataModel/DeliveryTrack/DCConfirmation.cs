using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.DeliveryTrack
{
    public class DCConfirmation : EntityBase
    {
        public DateTime DeliveryDate { get; set; }

        [StringLength(12)]
        public string DeliveryTime { get; set; }
    }
}
