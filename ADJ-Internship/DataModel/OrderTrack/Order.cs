using ADJ.Common;
using ADJ.DataModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace ADJ.DataModel.OrderTrack
{
    public class Order : EntityBase
    {
        public string PONumber { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(30)]
        public string Company { get; set; }

        [StringLength(30)]
        public string Supplier { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string PortOfLoading { get; set; }

        [Required]
        public string PortOfDelivery { get; set; }

        [StringLength(30)]
        public string Buyer { get; set; }

        [StringLength(30)]
        public string Department { get; set; }

        [StringLength(30)]
        public string OrderType { get; set; }

        public string Season { get; set; }

        [StringLength(30)]
        public string Factory { get; set; }

        Currency Currency = Currency.USD;




        public DateTime ShipDate { get; set; }

        public DateTime LatestShipDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string Mode { get; set; }

        [StringLength(30)]
        public string Vendor { get; set; }

        public float POQuantity { get; set; }

        Status Status = Status.New;
    }
}
