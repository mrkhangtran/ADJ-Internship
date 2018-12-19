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
    public class OrderService : ServiceBase, IOrderService
    {
        private readonly IDataProvider<Order> _orderDataProvider;
        private readonly IOrderRepository _orderRepository;

        private readonly IDataProvider<OrderDetail> _orderDetailProvider;
        private readonly IOrderDetailRepository _orderDetailRepository;

        private readonly IDataProvider<ProgressCheck> _progressCheckProvider;
        private readonly IProgressCheckRepository _progressCheckRepository;


        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IProgressCheckRepository progressCheckRepository, IDataProvider<Order> order, IDataProvider<OrderDetail> orderDetail, IDataProvider<ProgressCheck> progressCheck) : base(unitOfWork, mapper, appContext)
        {
            this._orderDataProvider = order;
            this._orderDetailProvider = orderDetail;
            this._progressCheckProvider = progressCheck;

            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._progressCheckRepository = progressCheckRepository;
        }

        //Get POs
        public async Task<List<OrderDisplayDto>> GetPOsAsync()
        {
            List<OrderDisplayDto> lstPO = new List<OrderDisplayDto>();
            var lstOrder = await _orderDataProvider.ListAsync();
            var lstOrderDetail = await _orderDetailProvider.ListAsync();                                 
            
            foreach(var i in lstOrder.Items)
            {
								OrderDisplayDto PO = new OrderDisplayDto();
                PO.PONumber = i.PONumber;
                PO.PODate = i.OrderDate;
                PO.Supplier = i.Supplier;
                PO.Origin = i.Origin;
                PO.PortOfLoading = i.PortOfLoading;
                PO.POShipDate = i.ShipDate;
                PO.PODeliveryDate = i.DeliveryDate;
                PO.PortOfDelivery = i.PortOfDelivery;
								PO.Status = i.Status;
                foreach(var j in lstOrderDetail.Items)
                {
                    if (j.OrderId == i.Id)
                    {
                        PO.POQuantity = j.Quantity;
                    }
                }
                lstPO.Add(PO);
            }
            return lstPO;        
        }

        //Filter POs
        public async Task<List<OrderDisplayDto>> FilterPO(string key)
        {
           
            List<OrderDisplayDto> lstPO = await GetPOsAsync();

            List<OrderDisplayDto> result = new List<OrderDisplayDto>();
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
