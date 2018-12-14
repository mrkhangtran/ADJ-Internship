﻿using ADJ.BusinessService.Core;
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
            IDataProvider<ProgressCheck> progresscheckDataProvider,IDataProvider<Order> orderDataProvider,
            IDataProvider<OrderDetail> orderdetailDataProvider, IProgressCheckRepository progresscheckRepository,
            IOrderRepository orderRepository,IOrderDetailRepository orderdetailRepository) : base(unitOfWork, mapper, appContext)
        {
            _orderDataProvider = orderDataProvider;
            _orderRepository = orderRepository;

            _orderdetailRepository = orderdetailRepository;
            _orderdetailDataProvider = orderdetailDataProvider;

            _progresscheckRepository = progresscheckRepository;
            _progresscheckDataProvider = progresscheckDataProvider;
        }
        public async Task<bool> CheckOrderHasProgress(int orderId)
        {
            List<ProgressCheck> progress = await _progresscheckRepository.Query(p=>p.OrderId==orderId,false).SelectAsync();
            bool result;
            if (progress == null)
            {
                 result = true;
                return result;
            }
             result = false;
            return result;          
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
             _progresscheckRepository.Insert(progressCheck);
            UnitOfWork.SaveChangesAsync();

        }
        public async Task<PagedListResult<ProgressCheckDto>> ListProgressCheckDtoAsync()
        {
            List<ProgressCheckDto> progressCheckDTOs = new List<ProgressCheckDto>();
            var lst = await _orderDataProvider.ListAsync();
            List<Order> orders = lst.Items;
            foreach (var order in orders)
            {
                float POQuantity = 0;
                List<OrderDetail> orderDetails = await _orderdetailRepository.Query(x => x.OrderId == order.Id,false).SelectAsync();
                foreach (var orderDetail in orderDetails)
                {
                    if (orderDetail.OrderId == order.Id)
                    {
                        POQuantity += orderDetail.Quantity;
                    }
                }
                List<ProgressCheck> lstProgress = new List<ProgressCheck>();
                lstProgress = await _progresscheckRepository.Query(x => x.OrderId == order.Id, false).SelectAsync();
                ProgressCheck progressCheck = new ProgressCheck();
                if (lstProgress==null)
                {
                    progressCheck.Id = 0;
                    progressCheck.InspectionDate = DateTime.Now.Date;
                    progressCheck.IntendedShipDate = DateTime.Now.Date;
                    progressCheck.Complete = false;
                    progressCheck.OrderId = order.Id;   
                }
                else
                {
                    progressCheck = lstProgress[0];
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
                    ListOrderDetail = orderDetails,
                    ListOrderDetailDto=Mapper.Map<List<OrderDetailDto>>(orderDetails),
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
            //await UnitOfWork.SaveChangesAsync();
            return lstProChDto;
        }
        //public async void Update(ProgressCheckDto progressCheckDTO)
        //{
        //    ProgressCheck check = await _progresscheckRepository.GetByIdAsync(progressCheckDTO.Id, false);
        //    check.InspectionDate = progressCheckDTO.InspectionDate;
        //    check.IntendedShipDate = progressCheckDTO.IntendedShipDate;
        //    float temp = 0;
        //    progressCheckDTO.ListOrderDetail = Mapper.Map<List<OrderDetail>>(progressCheckDTO.ListOrderDetailDto);
        //    foreach (var item in progressCheckDTO.ListOrderDetail) //MappingList
        //    {
        //        temp += item.ReviseQuantity;
        //        OrderDetail orderDetail = await _orderdetailRepository.GetByIdAsync(item.Id,false);
        //        orderDetail.ReviseQuantity = item.ReviseQuantity;
        //        _orderdetailRepository.Update(orderDetail);               
        //    }
        //    check.EstQtyToShip = (int)temp;
        //    if (check.EstQtyToShip == progressCheckDTO.POQuantity)
        //    {
        //        check.Complete = true;
        //    }
        //    else
        //    {
        //        check.Complete = false;
        //    }
        //    _progresscheckRepository.Update(check);
        //    await UnitOfWork.SaveChangesAsync();
        //}
        public async Task<ProgressCheckDto> CreateOrUpdatePurchaseOrderAsync(ProgressCheckDto rq)
        {
            ProgressCheck entity = new ProgressCheck();
            if (rq.Id > 0)
            {
                entity = await _progresscheckRepository.GetByIdAsync(rq.Id, false);
                if (entity == null)
                {
                    throw new AppException("Progress Check Not Found");
                }

                //entity = Mapper.Map(rq, entity);
                entity.InspectionDate = rq.InspectionDate;
                entity.IntendedShipDate = rq.IntendedShipDate;
                float temp = 0;
                rq.ListOrderDetail = Mapper.Map<List<OrderDetail>>(rq.ListOrderDetailDto);
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

                float temp = 0;
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
            var lst= await _orderDataProvider.ListAsync();
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
