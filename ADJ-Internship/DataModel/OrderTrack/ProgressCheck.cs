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

        public bool OnSchedule { get; set; }

        public DateTime IntendedShipDate { get; set; }

        public float EstQtyToShip { get; set; }

        public DateTime InspectionDate { get; set; }

        public string Comments { get; set; }

        [Required]
        public int OrderId { get; set; }
    }
}
