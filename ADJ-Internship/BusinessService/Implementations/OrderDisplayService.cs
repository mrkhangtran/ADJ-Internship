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
	public class OrderDisplayService : ServiceBase, IOrderDisplayService
	{
		private readonly IDataProvider<Order> _orderDataProvider;
		private readonly IOrderRepository _orderRepository;


		public OrderDisplayService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Order> orderDataProvider, IOrderRepository orderRepository) : base(unitOfWork, mapper, appContext)
		{
			this._orderDataProvider = orderDataProvider;
			this._orderRepository = orderRepository;


		}

		public async Task<List<OrderDto>> DisplaysAsync(string searchTerm)
		{
			var poResult = await _orderDataProvider.ListAsync();
			List<OrderDto> lstResult = new List<OrderDto>();
			List<OrderDto> lstFilterResult = new List<OrderDto>();

			foreach (var i in poResult.Items)
			{
				OrderDto orderDto = new OrderDto();
				orderDto.PONumber = i.PONumber;
				orderDto.OrderDate = i.OrderDate;
				orderDto.Supplier = i.Supplier;
				orderDto.Origin = i.Origin;
				orderDto.PortOfLoading = i.PortOfLoading;
				orderDto.ShipDate = i.ShipDate;
				orderDto.DeliveryDate = i.DeliveryDate;
				orderDto.PortOfDelivery = i.PortOfDelivery;
				orderDto.Status = i.Status;
				orderDto.Quantity = i.POQuantity;
				lstResult.Add(orderDto);
			}
			
			foreach (var j in lstResult)
			{
				if (j.PONumber == searchTerm)
				{
					lstFilterResult.Add(j);
				}
			}
			if (lstFilterResult.Count != 0)
			{
				return lstFilterResult;
			}
			else
			{
				return lstResult;
			}
			

		}
	}
}
