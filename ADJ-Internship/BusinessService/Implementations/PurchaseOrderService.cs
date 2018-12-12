using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.BusinessService.Validators;
using ADJ.Common;
using ADJ.DataModel;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using FluentValidation;

namespace ADJ.BusinessService.Implementations
{
    public class PurchaseOrderService : ServiceBase, IPurchaseOrderService
    {
        /*private readonly IDataProvider<PurchaseOrder> _poDataProvider;
        private readonly IPurchaseOrderRepository _poRepository;*/

        private readonly IDataProvider<Order> _orderDataProvider;
        private readonly IOrderRepository _orderRepository;

        private readonly IDataProvider<OrderDetail> _orderDetailDataProvider;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public PurchaseOrderService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Order> poDataProvider, IOrderRepository poRepository, IDataProvider<OrderDetail> poDetailDataProvider, IOrderDetailRepository poDetailRepository) : base(unitOfWork, mapper, appContext)
        {
            _orderDataProvider = poDataProvider;
            _orderRepository = poRepository;
            _orderDetailDataProvider = poDetailDataProvider;
            _orderDetailRepository = poDetailRepository;
        }

        public async Task<PagedListResult<OrderDTO>> ListOrderAsync(string searchTerm)
        {
            var poResult = await _orderDataProvider.ListAsync();
            var result = new PagedListResult<OrderDTO>
            {
                TotalCount = poResult.TotalCount,
                PageCount = poResult.PageCount,
                Items = Mapper.Map<List<OrderDTO>>(poResult.Items)
            };

            return result;
        }

        public async Task<OrderDTO> CreateOrUpdateOrderAsync(OrderDTO order)
        {
            Order entity;
            if (order.Id > 0)
            {
                entity = await _orderRepository.GetByIdAsync(order.Id, true);
                if (entity == null)
                {
                    throw new AppException("Purchase Order Not Found");
                }

                entity = Mapper.Map(order, entity);
                _orderRepository.Update(entity);
            }
            else
            {
                entity = Mapper.Map<Order>(order);

                _orderRepository.Insert(entity);
            }

            await UnitOfWork.SaveChangesAsync();

            var rs = Mapper.Map<OrderDTO>(entity);
            return rs;
        }

        public async Task<int> GetLastOrderId()
        {
            var orderResult = await _orderDataProvider.ListAsync();
            List<Order> orders = orderResult.Items;

            return orders[orders.Count - 1].Id;
        }

        public async Task<OrderDetailDTO> CreateOrUpdateOrderDetailAsync(OrderDetailDTO orderDetail)
        {
            OrderDetail entity;
            if (orderDetail.Id > 0)
            {
                entity = await _orderDetailRepository.GetByIdAsync(orderDetail.Id, true);
                if (entity == null)
                {
                    throw new AppException("Purchase Order Detail Not Found");
                }

                entity = Mapper.Map(orderDetail, entity);
                _orderDetailRepository.Update(entity);
            }
            else
            {
                entity = Mapper.Map<OrderDetail>(orderDetail);
                _orderDetailRepository.Insert(entity);
            }

            await UnitOfWork.SaveChangesAsync();

            var rs = Mapper.Map<OrderDetailDTO>(entity);
            return rs;
        }

        public async Task<bool> UniquePONumAsync(string PONumber, int? id)
        {
            var orderResult = await _orderDataProvider.ListAsync();
            List<Order> orders = orderResult.Items;

            if (id == null || id == 0)
            {
                foreach (var item in orders)
                {
                    if (item.PONumber == PONumber)
                    {
                        return false;
                    }
                }
            }
            else
            {
                foreach (var item in orders)
                {
                    if ((item.PONumber == PONumber) && (item.Id != id))
                    {
                        return false;
                    }
                }
            }
       
            return true;
        }

        public async Task<bool> UniqueItemNumAsync(string itemNum, int? id)
        {
            var orderDetailResult = await _orderDetailDataProvider.ListAsync();
            List<OrderDetail> orderDetails = orderDetailResult.Items;

            if (id == null || id == 0)
            {
                foreach (var item in orderDetails)
                {
                    if (item.ItemNumber == itemNum)
                    {
                        return false;
                    }
                }
            }
            else
            {
                foreach (var item in orderDetails)
                {
                    if ((item.ItemNumber == itemNum) && (item.Id != id))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /*public PurchaseOrderService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<PurchaseOrder> poDataProvider, IPurchaseOrderRepository poRepository) : base(unitOfWork, mapper, appContext)
        {
            _poDataProvider = poDataProvider;
            _poRepository = poRepository;
        }

        public async Task<PagedListResult<PurchaseOrderDto>> ListPurchaseOrdersAsync(string searchTerm)
        {
            var poResult = await _poDataProvider.ListAsync();
            var result = new PagedListResult<PurchaseOrderDto>
            {
                TotalCount = poResult.TotalCount,
                PageCount = poResult.PageCount,
                Items = Mapper.Map<List<PurchaseOrderDto>>(poResult.Items)
            };

            return result;
        }

        public async Task<PurchaseOrderDto> CreateOrUpdatePurchaseOrderAsync(CreateOrUpdatePurchaseOrderRq rq)
        {
            var validator = new CreateOrUpdatePurchaseOrderRqValidator();
            await validator.ValidateAndThrowAsync(rq);

            PurchaseOrder entity;
            if (rq.Id > 0)
            {
                entity = await _poRepository.GetByIdAsync(rq.Id, true);
                if (entity == null)
                {
                    throw new AppException("Purchase Order Not Found");
                }

                entity = Mapper.Map(rq, entity);
                _poRepository.Update(entity);
            }
            else
            {
                entity = Mapper.Map<PurchaseOrder>(rq);
                _poRepository.Insert(entity);
            }

            await UnitOfWork.SaveChangesAsync();

            var rs = Mapper.Map<PurchaseOrderDto>(entity);
            return rs;
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            _poRepository.Delete(id);
            await UnitOfWork.SaveChangesAsync();
        }*/
    }
}
