using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ADJ.BusinessService.Core;
using ADJ.DataModel;
using ADJ.DataModel.Core;
using ADJ.DataModel.OrderTrack;
using AutoMapper;

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
    //get data from Order model
    public class OrderDto : EntityDtoBase, ICreateMapping
    {
        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<Order, OrderDto>().IncludeBase<EntityBase, EntityDtoBase>();
            profile.CreateMap<OrderDto, Order>().IncludeBase<EntityDtoBase, EntityBase>();
        }
        public string PONumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime OrderDate { get; set; }

        public string Supplier { get; set; }

        public string Origin { get; set; }

        public string PortOfLoading { get; set; }

        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime ShipDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:DD/MM/YYYY}")]
        public DateTime DeliveryDate { get; set; }

        public string PortOfDelivery { get; set; }

    }

    //get data from OrderDetail model
    public class OrderDetailDto : EntityDtoBase, ICreateMapping
    {
        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<OrderDetail, OrderDetailDto>().IncludeBase<EntityBase, EntityDtoBase>();
            profile.CreateMap<OrderDetailDto, OrderDetail>().IncludeBase<EntityDtoBase, EntityBase>();
        }
        public float Quantaity { get; set; }

    }
    //get data from ProgressCheck
    public class ProgressCheckDto : EntityDtoBase, ICreateMapping
    {
        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<ProgressCheck, ProgressCheckDto>().IncludeBase<EntityBase, EntityDtoBase>();
            profile.CreateMap<ProgressCheckDto, ProgressCheck>().IncludeBase<EntityDtoBase, EntityBase>();
        }
        public  bool Complete { get; set; }

        public string Status
        {
            get
            {
                return Status;
            }
            set
            {
                if (this.Complete == false)
                {
                    value = "UnBooked";
                }
                else
                {
                    value = "Booked";
                }
            }
        }
    }  



}
