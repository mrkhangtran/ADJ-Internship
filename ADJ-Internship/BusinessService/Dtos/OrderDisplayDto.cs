using ADJ.BusinessService.Core;
using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.DataModel.OrderTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
    public class OrderDisplayDto : EntityDtoBase
    {
        [DisplayName("PO Number")]
        public string PONumber { get; set; }

        [DisplayName("PO Date")]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime PODate { get; set; }

        public string Supplier { get; set; }

        public string Origin { get; set; }

        [DisplayName("Port Of Loading")]
        public string PortOfLoading { get; set; }

        [DisplayName("PO Ship Date")]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime POShipDate { get; set; }

        [DisplayName("PO Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime PODeliveryDate { get; set; }

        [DisplayName("Port Of Delivery")]
        public string PortOfDelivery { get; set; }

        [DisplayName("PO Quantity")]
        public decimal POQuantity { get; set; }

        public OrderStatus Status { get; set; }

        


    }
}
