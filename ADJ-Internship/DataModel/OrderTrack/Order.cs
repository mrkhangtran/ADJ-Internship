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
        //DropList from 2010 to 2020
        //Default value is Current Date
        //[DataType(DataType.DateTime)]
        //[Display(Name = "Order Date")]
        //[DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(30)]
        public string Company { get; set; }

        [StringLength(30)]
        public string Supplier { get; set; }

        //Droplist Vietnam-HongKong
        [Required]
        public string Origin { get; set; }

        [Required]
        //[Display(Name = "Port of Loading")]
        public string PortOfLoading { get; set; }

        [Required]
        //[Display(Name = "Port of Delivery")]
        public string PortOfDelivery { get; set; }

        [StringLength(30)]
        public string Buyer { get; set; }

        [StringLength(30)]
        public string Department { get; set; }

        //[Display(Name = "Order Type")]
        [StringLength(30)]
        public string OrderType { get; set; }

        //Droplist from 2010 to 2020
        public string Season { get; set; }

        [StringLength(30)]
        public string Factory { get; set; }

        //Default value = USD
        public string Currency { get; set; } = "USD";

        //[Display(Name = "Ship Date")]
        public DateTime ShipDate { get; set; }

        //[Display(Name = "Latest Ship Date")]
        public DateTime LatestShipDate { get; set; }

        //[Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

        //DropList Road-Sea-Air
        public string Mode { get; set; }

        [StringLength(30)]
        public string Vendor { get; set; }

        //sum of all PODetails Quantity 
        public float POQuantity { get; set; }

        //Default value = "New"
        public string Status { get; set; } = "New";

        public ICollection<OrderDetail> orderDetails { get; set; }
    }
}
