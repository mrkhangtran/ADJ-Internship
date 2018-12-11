using System;
using System.Collections.Generic;
using System.Text;
using ADJ.BusinessService.Core;
using ADJ.DataModel;
using ADJ.DataModel.Core;
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
}
