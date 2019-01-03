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
			string sortStr = "OrderDate DESC";
			var poResult = await _orderDataProvider.ListAsync(query, sortStr, true, pageIndex, pageSize);

			var pagedResult = new PagedListResult<OrderDto>
			{
				TotalCount = poResult.TotalCount,
				PageCount = poResult.PageCount,
				CurrentFilter = poNumber,
				Items = Mapper.Map<List<OrderDto>>(poResult.Items)
			};


			return pagedResult;


		}

	}
}
