using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
	public class OrderDisplayService : ServiceBase, IOrderDisplayService
	{
		private readonly IDataProvider<Order> _orderDataProvider;
		private readonly IOrderRepository _orderRepository;


		public OrderDisplayService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Order> orderDataProvider, IOrderRepository orderRepository) : base(unitOfWork, mapper, appContext)
		{
			this._orderDataProvider = orderDataProvider;
			this._orderRepository = orderRepository;


		}

		public async Task<PagedListResult<OrderDto>> DisplaysAsync(string poNumber, int? pageIndex, int? pageSize)
		{
			Expression<Func<Order, bool>> query;
			if (poNumber == null)
			{
				query = null;
			}
			else
			{
				query = (p => p.PONumber == poNumber);
			}

			var poResult = await _orderDataProvider.ListAsync(query, null, true, pageIndex, pageSize);

			var pagedResult = new PagedListResult<OrderDto>
			{
				TotalCount = poResult.TotalCount,
				PageCount = poResult.PageCount,
				Items = Mapper.Map<List<OrderDto>>(poResult.Items)
			};

			
			return pagedResult;


































			//var poResult = await _orderDataProvider.ListAsync();
			//PagedListResult<OrderDto> result = new PagedListResult<OrderDto>();
			//result.TotalCount = poResult.TotalCount;
			//result.PageCount = poResult.PageCount;

			//List<OrderDto> lstResult = new List<OrderDto>();
			//List<OrderDto> lstFilterResult = new List<OrderDto>();

			//foreach (var i in poResult.Items)
			//{
			//	OrderDto orderDto = new OrderDto();
			//	orderDto.PONumber = i.PONumber;
			//	orderDto.OrderDate = i.OrderDate;
			//	orderDto.Supplier = i.Supplier;
			//	orderDto.Origin = i.Origin;
			//	orderDto.PortOfLoading = i.PortOfLoading;
			//	orderDto.ShipDate = i.ShipDate;
			//	orderDto.DeliveryDate = i.DeliveryDate;
			//	orderDto.PortOfDelivery = i.PortOfDelivery;
			//	orderDto.Status = i.Status;
			//	orderDto.Quantity = i.POQuantity;
			//	lstResult.Add(orderDto);
			//}

			//foreach (var j in lstResult)
			//{
			//	if (j.PONumber == poNumber)
			//	{
			//		lstFilterResult.Add(j);
			//	}
			//}
			//if (lstFilterResult.Count != 0)
			//{
			//	result.Items= lstFilterResult;
			//}
			//else
			//{
			//	result.Items =lstResult;
			//}
			//return result;

		}


	}
}
