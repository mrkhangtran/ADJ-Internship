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
	public class VesselDepartureService : ServiceBase, IVesselDepartureService
	{
		private readonly IDataProvider<Order> _orderDataProvider;
		private readonly IOrderRepository _orderRepository;


		public VesselDepartureService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Order> orderDataProvider, IOrderRepository orderRepository): base (unitOfWork, mapper, appContext)
		{
			this._orderDataProvider = orderDataProvider;
			this._orderRepository = orderRepository;
		}

		public async Task<GetItemSearchDto> SearchItem()
		{
			var lst = await _orderDataProvider.ListAsync();
			List<Order> orderModels = lst.Items;
			var origins = orderModels.Select(x => x.Origin).Distinct();
			var originports = orderModels.Select(x => x.PortOfLoading).Distinct();
			var vendors = orderModels.Select(x => x.Vendor).Distinct();
			GetItemSearchDto getSearchItemDTO = new GetItemSearchDto()
			{
				Origins = origins,
				OriginPorts = originports,
				Factories = vendors,
			};
			return getSearchItemDTO;
		}
	}
}
