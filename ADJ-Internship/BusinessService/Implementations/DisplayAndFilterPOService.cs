using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using FluentValidation;

namespace ADJ.BusinessService.Implementations
{
    public class DisplayAndFilterPOService : ServiceBase, IDisplayAndFilterService
    {
        private readonly IDataProvider<Order> _order;
        private readonly IOrderRepository _orderRepository;

        private readonly IDataProvider<OrderDetail> _orderDetail;
        private readonly IOrderDetailRepository _orderDetailRepository;

        private readonly IDataProvider<ProgressCheck> _progressCheck;
        private readonly IProgressCheckRepository _progressCheckRepository;


        public DisplayAndFilterPOService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IProgressCheckRepository progressCheckRepository, IDataProvider<Order> order, IDataProvider<OrderDetail> orderDetail, IDataProvider<ProgressCheck> progressCheck) : base(unitOfWork, mapper, appContext)
        {
            this._order = order;
            this._orderDetail = orderDetail;
            this._progressCheck = progressCheck;

            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._progressCheckRepository = progressCheckRepository;
        }

        //Get POs
        public async Task<List<PODisplayDto>> GetPOsAsync()
        {
            List<PODisplayDto> lstPO = new List<PODisplayDto>();
            var lstOrder = await _order.ListAsync();
            var lstOrderDetail = await _orderDetail.ListAsync();
            var lstProgressCheck = await _progressCheck.ListAsync();
            
            
            
            foreach(var i in lstOrder.Items)
            {
                PODisplayDto PO = new PODisplayDto();
                PO.PONumber = i.PONumber;
                PO.PODate = i.OrderDate;
                PO.Supplier = i.Supplier;
                PO.Origin = i.Origin;
                PO.PortOfLoading = i.PortOfLoading;
                PO.POShipDate = i.ShipDate;
                PO.PODeliveryDate = i.DeliveryDate;
                PO.PortOfDelivery = i.PortOfDelivery;
                foreach(var j in lstOrderDetail.Items)
                {
                    if (j.OrderId == i.Id)
                    {
                        PO.POQuantity = j.Quantity;
                    }
                }
                foreach(var x in lstProgressCheck.Items)
                {
                    if (x.OrderId == i.Id)
                    {
                        if (x.Complete == true)
                        {
                            PO.Status = "Booked";
                        }
                    }
                }
                lstPO.Add(PO);
            }
            return lstPO;        
        }

        //Filter POs
        public async Task<List<PODisplayDto>> FilterPO(string key)
        {
           
            List<PODisplayDto> lstPO = await GetPOsAsync();

            List<PODisplayDto> result = new List<PODisplayDto>();
            foreach (var i in lstPO)
            {
                if (i.PONumber == key)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        


    }
}
