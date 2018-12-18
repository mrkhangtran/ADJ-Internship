using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
    public class ProgressCheckService : ServiceBase, IProgressCheckService
    {
        private readonly IDataProvider<Order> _orderDataProvider;
        private readonly IOrderRepository _orderRepository;

        private readonly IOrderDetailRepository _orderdetailRepository;
        private readonly IDataProvider<OrderDetail> _orderdetailDataProvider;

        private readonly IDataProvider<ProgressCheck> _progresscheckDataProvider;
        private readonly IProgressCheckRepository _progresscheckRepository;

        public ProgressCheckService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
            IDataProvider<ProgressCheck> progresscheckDataProvider, IDataProvider<Order> orderDataProvider,
            IDataProvider<OrderDetail> orderdetailDataProvider, IProgressCheckRepository progresscheckRepository,
            IOrderRepository orderRepository, IOrderDetailRepository orderdetailRepository) : base(unitOfWork, mapper, appContext)
        {
            _orderDataProvider = orderDataProvider;
            _orderRepository = orderRepository;

            _orderdetailRepository = orderdetailRepository;
            _orderdetailDataProvider = orderdetailDataProvider;

            _progresscheckRepository = progresscheckRepository;
            _progresscheckDataProvider = progresscheckDataProvider;
        }
        public async Task<PagedListResult<ProgressCheckDto>> ListProgressCheckDtoAsync(int pageIndex=1, int pageSize=2)
        {
            List<ProgressCheckDto> progressCheckDTOs = new List<ProgressCheckDto>();
            var GetPageResult = await _orderDataProvider.ListAsync(null,null,true,pageIndex,pageSize);
            List<Order> orders = GetPageResult.Items;
            foreach (var order in orders)
            {
                decimal POQuantity = 0;
                foreach (var orderDetail in order.orderDetails)
                {                   
                        POQuantity += orderDetail.Quantity;                  
                }
                ProgressCheck progressCheck = new ProgressCheck();
                ProgressCheck check =  _progresscheckRepository.GetProgressCheckByOrderId(order.Id);
                if (check==null)
                {
                    progressCheck.Id = 0;
                    progressCheck.InspectionDate = DateTime.Now.Date;
                    progressCheck.IntendedShipDate = DateTime.Now.Date;
                    progressCheck.Complete = false;
                    progressCheck.OrderId = order.Id;
                }
                else
                {
                    progressCheck = check;
                    if (progressCheck.EstQtyToShip != POQuantity)
                    {
                        progressCheck.Complete = false;
                    }
                }
                ProgressCheckDto temp = new ProgressCheckDto()
                {
                    Id = progressCheck.Id,
                    Factory = order.Factory,
                    PONumber = order.PONumber,
                    ShipDate = order.ShipDate,
                    InspectionDate = progressCheck.InspectionDate,
                    IntendedShipDate = progressCheck.IntendedShipDate,
                    Complete = progressCheck.Complete,
                    POQuantity = POQuantity,
                    EstQtyToShip = progressCheck.EstQtyToShip,
                    Supplier = order.Supplier,                  
                    ListOrderDetailDto = Mapper.Map<List<OrderDetailDto>>(order.orderDetails),
                    OrderId = order.Id,
                    Origin = order.Origin,
                    OriginPort = order.PortOfDelivery,
                    Department = order.Department
                };
                progressCheckDTOs.Add(temp);
            }
            PagedListResult<ProgressCheckDto> lstProChDto = new PagedListResult<ProgressCheckDto>
            {
                TotalCount = GetPageResult.TotalCount,
                PageCount = GetPageResult.PageCount,
                Items = progressCheckDTOs
            };
            return lstProChDto;
        }
        public async Task<ProgressCheckDto> CreateOrUpdatePurchaseOrderAsync(ProgressCheckDto rq)
        {
            ProgressCheck entity = new ProgressCheck();
            rq.ListOrderDetail = Mapper.Map<List<OrderDetail>>(rq.ListOrderDetailDto);
            if (rq.Id > 0)
            {
                entity = await _progresscheckRepository.GetByIdAsync(rq.Id, false);
                if (entity == null)
                {
                    throw new AppException("Progress Check Not Found");
                }
                entity.InspectionDate = rq.InspectionDate;
                entity.IntendedShipDate = rq.IntendedShipDate;
                decimal temp = 0;

                foreach (var item in rq.ListOrderDetail)
                {
                    temp += item.ReviseQuantity;
                    OrderDetail orderDetail = await _orderdetailRepository.GetByIdAsync(item.Id, false);
                    orderDetail.ReviseQuantity = item.ReviseQuantity;
                    _orderdetailRepository.Update(orderDetail);
                }
                entity.EstQtyToShip = temp;
                if (entity.EstQtyToShip == rq.POQuantity)
                {
                    entity.Complete = true;
                }
                else
                {
                    entity.Complete = false;
                }
                _progresscheckRepository.Update(entity);
            }
            else
            {
                entity.InspectionDate = rq.InspectionDate;
                entity.IntendedShipDate = rq.IntendedShipDate;
                entity.OrderId = rq.OrderId;
                decimal temp = 0;
                foreach (var item in rq.ListOrderDetail)
                {
                    temp += item.ReviseQuantity;
                    OrderDetail orderDetail = await _orderdetailRepository.GetByIdAsync(item.Id, false);
                    orderDetail.ReviseQuantity = item.ReviseQuantity;
                    _orderdetailRepository.Update(orderDetail);
                }
                entity.EstQtyToShip = temp;
                if (entity.EstQtyToShip == rq.POQuantity)
                {
                    entity.Complete = true;
                }
                else
                {
                    entity.Complete = false;
                }
                _progresscheckRepository.Insert(entity);
            }

            await UnitOfWork.SaveChangesAsync();

            var rs = Mapper.Map<ProgressCheckDto>(entity);
            return rs;
        }
        public async Task<GetItemSearchDto> SearchItem()
        {
            var lst = await _orderDataProvider.ListAsync();
            List<Order> orderModels = lst.Items;
            var suppliers = orderModels.Select(x => x.Supplier).Distinct();
            var origins = orderModels.Select(x => x.Origin).Distinct();
            var originports = orderModels.Select(x => x.PortOfLoading).Distinct();
            var factories = orderModels.Select(x => x.Factory).Distinct();
            var depts = orderModels.Select(x => x.Department).Distinct();
            GetItemSearchDto getSearchItemDTO = new GetItemSearchDto()
            {
                Suppliers = suppliers,
                Origins = origins,
                OriginPorts = originports,
                Factories = factories,
                Depts = depts
            };
            return getSearchItemDTO;
        }
    }
}
