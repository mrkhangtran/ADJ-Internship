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
				entity = await _orderRepository.GetByIdAsync(order.Id, false);
				if (entity == null)
				{
					throw new AppException("Purchase Order Not Found");
				}

				entity = Mapper.Map(order, entity);

				//entity = Mapper.Map<Order>(order);
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

		public async Task<OrderDetailDTO> CreateOrUpdateOrderDetailAsync(OrderDetailDTO orderDetail)
		{
			OrderDetail entity;
			if (orderDetail.Id > 0)
			{
				entity = await _orderDetailRepository.GetByIdAsync(orderDetail.Id, false);
				if (entity == null)
				{
					throw new AppException("Purchase Order Detail Not Found");
				}

				entity = Mapper.Map(orderDetail, entity);

				//entity = Mapper.Map<OrderDetail>(orderDetail);

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

		public async Task<List<OrderDetailDTO>> DeleteOrderDetailAsync(List<OrderDetailDTO> orderDetailsFromView, int orderId)
		{
			List<OrderDetail> orderDetailsFromDB = await _orderDetailRepository.Query(x => x.OrderId == orderId, false).SelectAsync();
			List<OrderDetailDTO> deletedItems = new List<OrderDetailDTO>();

			bool delete;
			foreach (var item1 in orderDetailsFromDB)
			{
				delete = true;
				foreach (var item2 in orderDetailsFromView)
				{
					if ((item2.Id != 0) && (item1.Id == item2.Id))
					{
						delete = false;
					}
				}
				if (delete)
				{
					_orderDetailRepository.Delete(item1);
					deletedItems.Add(Mapper.Map<OrderDetailDTO>(item1));
				}
			}

			return deletedItems;
		}


		public async Task<bool> UniquePONumAsync(string PONumber, int? id)
		{
			List<Order> orders = new List<Order>();
			if ((id == null) || (id == 0)) { orders = await _orderRepository.Query(x => x.PONumber == PONumber, false).SelectAsync(); }
			else { orders = await _orderRepository.Query(x => x.PONumber == PONumber && x.Id != id, false).SelectAsync(); }

			if (orders.Count > 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public async Task<bool> UniqueItemNumAsync(string itemNum, int? id)
		{
			List<OrderDetail> orderDetails = new List<OrderDetail>();
			if ((id == null) || (id == 0)) { orderDetails = await _orderDetailRepository.Query(x => x.ItemNumber == itemNum, false).SelectAsync(); }
			else { orderDetails = await _orderDetailRepository.Query(x => x.ItemNumber == itemNum && x.Id != id, false).SelectAsync(); }

			if (orderDetails.Count > 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public async Task<OrderDTO> GetOrderByPONumber(string poNumber)
		{
			List<Order> orders = await _orderRepository.Query(x => x.PONumber == poNumber, true).SelectAsync();

			OrderDTO result = Mapper.Map<OrderDTO>(orders[0]);
			result.PODetails = new PagedListResult<OrderDetailDTO>();
			result.PODetails.Items = new List<OrderDetailDTO>();
			foreach (var item in orders[0].OrderDetails)
			{
				result.PODetails.Items.Add(Mapper.Map<OrderDetailDTO>(item));
			}

			result.PODetails.PageCount = 1;
			result.PODetails.TotalCount = 2;

			return result;
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
