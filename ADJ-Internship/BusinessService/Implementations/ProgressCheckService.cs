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
        private readonly IDataProvider<Order> _odDataProvider;
        private readonly IOrderRepository _odRepository;

        private readonly IOrderDetailRepository _oddRepository;
        private readonly IDataProvider<OrderDetail> _oddDataProvider;

        private readonly IDataProvider<ProgressCheck> _prcDataProvider;
        private readonly IProgressCheckRepository _prcRepository;

        public ProgressCheckService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
            IDataProvider<ProgressCheck> prcDataProvider,IDataProvider<Order> odDataProvider,
            IDataProvider<OrderDetail> oddDataProvider, IProgressCheckRepository prcRepository,
            IOrderRepository odRepository,IOrderDetailRepository oddRepository) : base(unitOfWork, mapper, appContext)
        {
            _odDataProvider = odDataProvider;
            _odRepository = odRepository;

            _oddRepository = oddRepository;
            _oddDataProvider = oddDataProvider;

            _prcRepository = prcRepository;
            _prcDataProvider = prcDataProvider;
        }
        public async Task<int> CheckOrderHaventProgress(int orderId)
        {
            var result = await _prcDataProvider.ListAsync();
            List<ProgressCheck> prcList = result.Items;
            int check = 0;
            foreach (var item in prcList)
            {
                if (item.OrderId == orderId)
                {
                    check += 1;                  
                }
            }
            return check;
        }
        public void CreateDefaultModel(int orderId)
        {
            ProgressCheck progressCheck = new ProgressCheck
            {
                InspectionDate = DateTime.Now.Date,
                IntendedShipDate = DateTime.Now.Date,
                Complete = false,
                OrderId = orderId
            };
             _prcRepository.Insert(progressCheck);
            UnitOfWork.SaveChangesAsync();

        }
        public async Task<PagedListResult<ProgressCheckDto>> ListProgressCheckDtoAsync()
        {
            List<ProgressCheckDto> progressCheckDTOs = new List<ProgressCheckDto>();
            var lstOrder = await _odDataProvider.ListAsync();
            var lstOrDetail =await _oddDataProvider.ListAsync();
            var lstProgress = await _prcDataProvider.ListAsync();
            List<Order> orders = lstOrder.Items;
            foreach (var order in orders)
            {
                if (await CheckOrderHaventProgress(order.Id) ==0)
                {
                    CreateDefaultModel(order.Id);
                }
                float POQuantity = 0;
                List<OrderDetail> orderDetails = lstOrDetail.Items;
                List<OrderDetail> orderDetailModels = new List<OrderDetail>();
                foreach (var orderDetail in orderDetails)
                {
                    if (orderDetail.OrderId == order.Id)
                    {
                        orderDetailModels.Add(orderDetail);
                        POQuantity += orderDetail.Quantity;
                    }
                }
                List<ProgressCheck> progressChecks = lstProgress.Items;
                ProgressCheck progressCheck = new ProgressCheck();
                foreach (var item in progressChecks)
                {
                    if (item.OrderId == order.Id)
                    {
                        progressCheck = item;
                    }
                }
                if (progressCheck.EstQtyToShip != POQuantity)
                {
                    progressCheck.Complete = false;
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
                    ListOrderDetail = orderDetailModels, //Pa
                    ListOrderDetailProgress=Mapper.Map<List<OrderDetailDto_Progress>>(orderDetailModels),
                    OrderId = order.Id,
                    Origin = order.Origin,
                    OriginPort = order.PortOfDelivery,
                    Department = order.Department
                };
                progressCheckDTOs.Add(temp);
            }

            PagedListResult<ProgressCheckDto> lstProChDto = new PagedListResult<ProgressCheckDto>
            {
                TotalCount = progressCheckDTOs.Count,
                PageCount = 2,
                Items = progressCheckDTOs
            };
            await UnitOfWork.SaveChangesAsync();
            return lstProChDto;
        }
        public async void Update(ProgressCheckDto progressCheckDTO)
        {
            var lstOrder = await _odDataProvider.ListAsync();
            List<Order> orders = lstOrder.Items;
            var lstOrDetail = await _oddDataProvider.ListAsync();
            List<OrderDetail> orderDetails = lstOrDetail.Items;
            var lstProgress = await _prcDataProvider.ListAsync();
            List<ProgressCheck> progressChecks = lstProgress.Items;
            //ProgressCheck check = await _prcRepository.GetByIdAsync(progressCheckDTO.Id, true);
            ProgressCheck check = new ProgressCheck();
            foreach (var item in progressChecks)
            {
                if (item.Id == progressCheckDTO.Id)
                {
                    check = item;
                }                
            }
            check.InspectionDate = progressCheckDTO.InspectionDate;
            check.IntendedShipDate = progressCheckDTO.IntendedShipDate;
            float temp = 0;
            progressCheckDTO.ListOrderDetail = Mapper.Map<List<OrderDetail>>(progressCheckDTO.ListOrderDetailProgress);
            foreach (var item in progressCheckDTO.ListOrderDetail) //MappingList
            {
                temp += item.ReviseQuantity;
                //OrderDetail orderDetail = await _oddRepository.GetByIdAsync(item.Id,true);
                OrderDetail orderDetail = new OrderDetail();
                foreach (var j in orderDetails)
                {
                    if (j.Id == item.Id)
                    {
                        orderDetail = j;
                    }
                }
                orderDetail.ReviseQuantity = item.ReviseQuantity;
                _oddRepository.Update(orderDetail);               
            }
            check.EstQtyToShip = (int)temp;
            if (check.EstQtyToShip == progressCheckDTO.POQuantity)
            {
                check.Complete = true;
            }
            else
            {
                check.Complete = false;
            }
            _prcRepository.Update(check);
        }
        public async Task<GetItemSearchDto> SearchItem()
        {
            var lstOrder = await _odDataProvider.ListAsync();
            List<Order> orderModels = lstOrder.Items;
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
