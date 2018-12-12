using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.DataModel.OrderTrack
{
    public class ProgressCheck : EntityBase
    {
        public bool Complete { get; set; }

        //yes/no
        public bool OnSchedule { get; set; }

        //calendar control
        public DateTime IntendedShipDate { get; set; }

        // = sum of all revise quantity?
        public float EstQtyToShip { get; set; }

        //calendar control
        public DateTime InspectionDate { get; set; }

        //max string length
        public string Comments { get; set; }

        [Required]
        //[ForeignKey("OrderModel")]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
