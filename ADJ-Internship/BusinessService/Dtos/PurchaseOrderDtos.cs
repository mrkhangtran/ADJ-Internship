using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ADJ.BusinessService.Core;
using ADJ.DataModel;
using ADJ.DataModel.Core;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADJ.BusinessService.Dtos
{
    public class PurchaseOrderDto : EntityDtoBase, ICreateMapping
    {
        public string Test { get; set; }

        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<PurchaseOrder, PurchaseOrderDto>().IncludeBase<EntityBase, EntityDtoBase>();
            profile.CreateMap<PurchaseOrderDto, PurchaseOrder>().IncludeBase<EntityDtoBase, EntityBase>();
        }
    }

    public class CreateOrUpdatePurchaseOrderRq : EntityDtoBase, ICreateMapping
    {
        public string Test { get; set; }

        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<CreateOrUpdatePurchaseOrderRq, PurchaseOrder>().IncludeBase<EntityDtoBase, EntityBase>();
        }
    }

    public class OrderDTO
    {
        public int Id { get; set; }

        [Display(Name = "PO Number")]
        [StringLength(10, ErrorMessage = "Cannot be longer than 10 characters")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only")]
        public string PONumber { get; set; }

        //DropList from 2010 to 2020
        //Default value is Current Date
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(30, ErrorMessage = "Cannot be longer than 30 character")]
        public string Buyer { get; set; }

        //Default value = USD
        public string Currency { get; set; } = "USD";

        //Droplist from 2010 to 2020
        public string Season { get; set; }
        public IEnumerable<SelectListItem> Seasons { get; set; }

        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string Department { get; set; }

        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string Vendor { get; set; }

        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string Company { get; set; }

        //Droplist Vietnam-HongKong
        [Required]
        public string Origin { get; set; }
        public IEnumerable<SelectListItem> Origins { get; set; }

        //Droplist of Ports
        [Required]
        [Display(Name = "Port of Loading")]
        [PortIsDifferent("PortOfDelivery")]
        public string PortOfLoading { get; set; }

        [Required]
        [Display(Name = "Port of Delivery")]
        [PortIsDifferent("PortOfLoading")]
        public string PortOfDelivery { get; set; }
        public IEnumerable<SelectListItem> Ports { get; set; }

        [Display(Name = "Order Type")]
        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string OrderType { get; set; }

        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string Factory { get; set; }

        //DropList Road-Sea-Air
        [Display(Name = "Ship Method")]
        public string Mode { get; set; }
        public IEnumerable<SelectListItem> Modes { get; set; }

        [Display(Name = "Ship Date")]
        public DateTime ShipDate { get; set; }

        [Display(Name = "Latest Ship Date")]
        [SimilarOrLaterThanShipDate("ShipDate")]
        public DateTime LatestShipDate { get; set; }

        [Display(Name = "Delivery Date")]
        [SimilarOrLaterThanShipDate("ShipDate")]
        public DateTime DeliveryDate { get; set; }

        //sum of all PODetails Quantity 
        public float POQuantity { get; set; }

        //Default value = "New"
        public string Status { get; set; } = "New";

        public OrderDetailDTO orderDetailDTO { get; set; }

        public virtual List<OrderDetailDTO> PODetails { get; set; }
    }
}
