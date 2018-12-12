using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ADJ.BusinessService.Core;
using ADJ.DataModel;
using ADJ.DataModel.Core;
using ADJ.DataModel.OrderTrack;
using AutoMapper;

namespace ADJ.BusinessService.Dtos
{
    public class ProgressCheckDto : EntityDtoBase, ICreateMapping
    {
        [Display(Name = "PO Number")]
        public string PONumber { get; set; }
        [Display(Name = "PO Quantity")]
        public float POQuantity { get; set; }
        [Display(Name = "PO Check Quantity")]
        public float POCheckQuantity { get; set; }
        [Display(Name = "PO Ship Date")]
        public DateTime ShipDate { get; set; }
        public List<OrderDetail> ListOrderDetail { get; set; }
        [Display(Name = "Inspection Date")]
        public DateTime InspectionDate { get; set; }
        [Display(Name = "Int Ship Date")]
        public DateTime IntendedShipDate { get; set; }
        [Display(Name = "PO Quantity Complete ")]
        public bool Complete { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public bool OnSchedule { get; set; }
        public int OrderId { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public string Origin { get; set; }
        public string OriginPort { get; set; }

        public void CreateMapping(Profile profile)
        {
            profile.CreateMap<ProgressCheck, ProgressCheckDto>().IncludeBase<EntityBase, EntityDtoBase>();
            profile.CreateMap<ProgressCheckDto, ProgressCheck>().IncludeBase<EntityDtoBase, EntityBase>();
        }
    }
    public class GetItemSearchDto : EntityDtoBase
    {
        public IEnumerable<string> Origins { get; set; }
        public IEnumerable<string> OriginPorts { get; set; }
        public IEnumerable<string> Factories { get; set; }
        public IEnumerable<string> Suppliers { get; set; }
        public IEnumerable<string> Status { get; set; }
        public IEnumerable<string> Depts { get; set; }
        
    }
}
