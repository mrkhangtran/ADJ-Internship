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
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using FluentValidation;
using LinqKit;
using static ADJ.BusinessService.Dtos.VesselDepartureDtos;

namespace ADJ.BusinessService.Implementations
{
	public class VesselDepartureService : ServiceBase, IVesselDepartureService
	{
		private readonly IDataProvider<Order> _orderDataProvider;
		private readonly IOrderRepository _orderRepository;

		private readonly IDataProvider<Container> _containerDataProvider;
		private readonly IContainerRepository _containerRepository;

		private readonly IDataProvider<Manifest> _manifestDataProvider;
		private readonly IManifestRepository _manifestRepository;

		private readonly IDataProvider<Booking> _bookingDataProvider;
		private readonly IBookingRepository _bookingRepository;

		public VesselDepartureService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
			IDataProvider<Order> orderDataProvider, IDataProvider<Container> containerDataProvider, IDataProvider<Manifest> manifestDataProvider, IDataProvider<Booking> bookingDataProvider,
			IOrderRepository orderRepository, IContainerRepository containerRepository, IManifestRepository manifestRepository, IBookingRepository bookingRepository)
			: base(unitOfWork, mapper, appContext)
		{
			this._orderDataProvider = orderDataProvider;
			this._orderRepository = orderRepository;

			this._containerDataProvider = containerDataProvider;
			this._containerRepository = containerRepository;

			this._manifestDataProvider = manifestDataProvider;
			this._manifestRepository = manifestRepository;

			this._bookingDataProvider = bookingDataProvider;
			this._bookingRepository = bookingRepository;

		}

		public async Task<GetItemSearchDto> SearchItem()
		{
			var lst = await _orderDataProvider.ListAsync();
			List<Order> orderModels = lst.Items;
			var origins = orderModels.Select(x => x.Origin).Distinct();
			var originports = orderModels.Select(x => x.PortOfLoading).Distinct();
			var vendors = orderModels.Select(x => x.Vendor).Distinct();
			var destports = orderModels.Select(x => x.PortOfDelivery).Distinct();
			var modes = orderModels.Select(x => x.Mode).Distinct();

			GetItemSearchDto getSearchItemDTO = new GetItemSearchDto()
			{
				Origins = origins,
				OriginPorts = originports,
				Factories = vendors,
				DestPorts = destports,
				Modes = modes
			};
			return getSearchItemDTO;
		}

		public async Task<PagedListResult<ContainerDto>> pagedListContainerAsync(OrderStatus Status, DateTime ETDFroms, DateTime ETDTos,
			int pageIndex = 1, int pageSize = 2, string PONumberSearch = null,
			string ItemSearch = null, string Origins = null, string OriginPorts = null)
		{
			Expression<Func<Container, bool>> AllOfContainer = x => x.Id > 0;
			if (PONumberSearch != null)
			{
				// add condition x.PO==POSearch
				Expression<Func<Container, bool>> filterPO = x => x.Manifests.Where(p => p.Booking.PONumber == PONumberSearch).Count() > 0;
				//var test = a.And(a);
				AllOfContainer = AllOfContainer.And(filterPO);

			}
			if (Origins != null)
			{
				Expression<Func<Container, bool>> filterOrigin = x => x.Manifests.Where(p => p.Booking.Order.Origin == Origins).Count() > 0;
				AllOfContainer = AllOfContainer.And(filterOrigin);
			}
			if (OriginPorts != null)
			{
				// add condition x.port==port
				Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.Order.PortOfLoading == OriginPorts).Count() > 0;
				AllOfContainer = AllOfContainer.And(filterOriginPort);
			}
			if (ItemSearch != null)
			{
				Expression<Func<Container, bool>> filterItem = x => x.Manifests.Where(p => p.Booking.Order.OrderDetails.Where(i => i.ItemNumber == ItemSearch).Count() > 0).Count() > 0;   
				AllOfContainer = AllOfContainer.And(filterItem);
			}
			if (ETDFroms != null)
			{
				Expression<Func<Container, bool>> filterETDFrom = x => x.Manifests.Where(p => p.Booking.ETD >= ETDFroms).Count() > 0;
				AllOfContainer = AllOfContainer.And(filterETDFrom);
			}
			if (ETDTos != null)
			{
				Expression<Func<Container, bool>> filterETDTo = x => x.Manifests.Where(p => p.Booking.ETD <= ETDTos).Count() > 0;
				AllOfContainer = AllOfContainer.And(filterETDTo);
			}
			Expression<Func<Container, bool>> filterStatus = x => x.Manifests.Where(p => p.Booking.Order.Status == Status).Count() > 0;
			AllOfContainer = AllOfContainer.And(filterStatus);

			var GetPageResult = await _containerDataProvider.ListAsync(AllOfContainer, null, true, pageIndex, pageSize);


			string tempOfCurrentFilter;
			if (ItemSearch != null)
			{
				tempOfCurrentFilter = ItemSearch;
			}
			else if (PONumberSearch != null)
			{
				tempOfCurrentFilter = PONumberSearch;
			}
			var pagedResult = new PagedListResult<ContainerDto>
			{
				TotalCount = GetPageResult.TotalCount,
				PageCount = GetPageResult.PageCount,
				CurrentFilter = ItemSearch,
				Items = Mapper.Map<List<ContainerDto>>(GetPageResult.Items)
			};



			return pagedResult;
		}
	}
}


//public async Task<Boo> FillterVesselDepartureAsync(ContainerDto rq);
//		{
//			ProgressCheck entity = new ProgressCheck();
//			//rq.ListOrderDetail = Mapper.Map<List<OrderDetail>>(rq.ListOrderDetailDto);
//			if (rq.Id > 0)
//			{
//				entity = await _progresscheckRepository.GetByIdAsync(rq.Id, false);
//				if (entity == null)
//				{
//					throw new AppException("Progress Check Not Found");
//	}
//				if (rq.selected == true)
//				{
//					entity.InspectionDate = rq.InspectionDate;
//					entity.IntendedShipDate = rq.IntendedShipDate;
//				}
//decimal temp = 0;

//				foreach (var item in rq.ListOrderDetailDto)
//				{
//					OrderDetail orderDetail = await _orderdetailRepository.GetByIdAsync(item.Id, false);
//					if (item.selected == true)
//					{
//						temp += item.ReviseQuantity;
//						orderDetail.ReviseQuantity = item.ReviseQuantity;
//						if (orderDetail.Quantity == item.ReviseQuantity)
//						{
//							orderDetail.Status = OrderStatus.AwaitingBooking;
//						}
//						_orderdetailRepository.Update(orderDetail);
//					}
//					else
//					{
//						temp += orderDetail.ReviseQuantity;
//					}
//				}
//				entity.EstQtyToShip = temp;
//				if (entity.EstQtyToShip == rq.POQuantity)
//				{
//					entity.Complete = true;
//				}
//				else
//				{
//					entity.Complete = false;
//				}
//				_progresscheckRepository.Update(entity);
//			}
//			else
//			{
//				entity.InspectionDate = rq.InspectionDate;
//				entity.IntendedShipDate = rq.IntendedShipDate;
//				entity.OrderId = rq.OrderId;
//				decimal temp = 0;
//				foreach (var item in rq.ListOrderDetailDto)
//				{
//					OrderDetail orderDetail = await _orderdetailRepository.GetByIdAsync(item.Id, false);
//					if (item.selected == true)
//					{
//						temp += item.ReviseQuantity;
//						orderDetail.ReviseQuantity = item.ReviseQuantity;
//						if (orderDetail.Quantity == item.ReviseQuantity)
//						{
//							orderDetail.Status = OrderStatus.AwaitingBooking;
//						}
//						_orderdetailRepository.Update(orderDetail);
//					}
//					else
//					{
//						temp += orderDetail.ReviseQuantity;
//					}
//				}
//				entity.EstQtyToShip = temp;
//				if (entity.EstQtyToShip == rq.POQuantity)
//				{
//					entity.Complete = true;
//				}
//				else
//				{
//					entity.Complete = false;
//				}
//				_progresscheckRepository.Insert(entity);
//			}

//			await UnitOfWork.SaveChangesAsync();

//var rs = Mapper.Map<ProgressCheckDto>(entity);
//			return rs;
//		}


//	}
//}
