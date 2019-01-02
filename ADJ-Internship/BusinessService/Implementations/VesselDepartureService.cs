using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.OrderTrack;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
	public class VesselDepartureService : ServiceBase, IVesselDepartureService
	{

		private readonly IManifestRepository _manifestRepository;
		private readonly IDataProvider<Manifest> _manifestDataProvider;

		private readonly IShipmentBookingRepository _shipmentBookingRepository;
		private readonly IDataProvider<Booking> _shipmentBookingDataProvider;

		private readonly IContainerRepository _containerRepository;
		private readonly IDataProvider<Container> _containerDataProvider;

		private readonly IOrderDetailRepository _orderDetailRepository;
		private readonly IDataProvider<OrderDetail> _orderDetailDataProvider;

		public VesselDepartureService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, 
			IDataProvider<Manifest> manifestDataProvider, IManifestRepository manifestRepository, 
			IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository shipmentBookingRepository, IContainerRepository containerRepository, 
			IDataProvider<Container> containerDataProvider, IOrderDetailRepository orderDetailRepository, IDataProvider<OrderDetail> orderDetailDataProvider) : base(unitOfWork, mapper, appContext)
		{
			this._manifestDataProvider = manifestDataProvider;
			this._manifestRepository = manifestRepository;

			this._shipmentBookingDataProvider = bookingDataProvider;
			this._shipmentBookingRepository = shipmentBookingRepository;

			this._containerRepository = containerRepository;
			this._containerDataProvider = containerDataProvider;

			this._orderDetailRepository = orderDetailRepository;
			this._orderDetailDataProvider = orderDetailDataProvider;

		}
		public async Task<PagedListResult<ContainerDto>> ListManifestDtoAsync(string origin, string originPort, string container, string status, DateTime etdFrom, DateTime etdTo, int? pageIndex, int? pageSize)
		{
			
			Expression<Func<Container, bool>> All = x => x.Id > 0;

			if(origin==null&& originPort == null && container == null && status == null && etdFrom == null && etdTo == null)
			{
				All = null;
			}


			if (origin != null)
			{
				Expression<Func<Container, bool>> filterOrigin = x => x.Manifests.Where(p => p.Booking.Order.Origin == origin).Count() > 0;
				All = All.And(filterOrigin);
			}
			if (originPort != null)
			{
				Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.PortOfLoading == originPort).Count() > 0;
				All = All.And(filterOriginPort);
			}
			if (container != null)
			{
				Expression<Func<Container, bool>> filterContainer = x => x.Manifests.Where(p => p.Container.Id.CompareTo(container) > 0).Count() > 0;
				All = All.And(filterContainer);
			}
			if (etdFrom != null)
			{
				Expression<Func<Container, bool>> filterETDFrom = x => x.Manifests.Where(p => p.Booking.ETD >= etdFrom).Count() > 0;
				All = All.And(filterETDFrom);
			}
			if (etdTo != null)
			{
				Expression<Func<Container, bool>> filterETDTo = x => x.Manifests.Where(p => p.Booking.ETD <= etdTo).Count() > 0;
				All = All.And(filterETDTo);
			}
			if (status != null)
			{
				Expression<Func<Container, bool>> filterStatus = x => x.Status.ToString() == status;
				All = All.And(filterStatus);
			}

			//sort string
			string sortStr = "Status ASC";
			var containers = await _containerDataProvider.ListAsync(All, sortStr, true, pageIndex, pageSize);
			var bookings = await _shipmentBookingDataProvider.ListAsync(null, null, true);
			List<Booking> listBooking = bookings.Items;
			List<string> originPorts = listBooking.Select(p => p.PortOfLoading).ToList();
			List<DateTime> ETDs = listBooking.Select(p => p.ETD).ToList();
			List<string> destinationPorts = listBooking.Select(p => p.PortOfDelivery).ToList();

			List<ContainerDto> lstContainerResult = new List<ContainerDto>();

			foreach (var i in containers.Items)
			{
				ContainerDto containerDto = new ContainerDto();
				containerDto.Name = i.Name;
				containerDto.Size = i.Size;
				containerDto.Status = i.Status;
				containerDto.OriginPort = (i.Manifests.ToList())[0].Booking.PortOfLoading;
				containerDto.DestPort = (i.Manifests.ToList())[0].Booking.PortOfDelivery;
				containerDto.Carrier = (i.Manifests.ToList())[0].Booking.Carrier;
				containerDto.Voyage = (i.Manifests.ToList())[0].Booking.Voyage;

				lstContainerResult.Add(containerDto);
			}

			//Group By
			lstContainerResult = lstContainerResult.OrderBy(m => m.OriginPort).OrderBy(m => m.DestPort).OrderBy(m => m.Loading).OrderBy(m=>m.Carrier ).ToList();

			PagedListResult<ContainerDto> pagedlistResult = new PagedListResult<ContainerDto>()
			{
				PageCount=containers.PageCount,
				TotalCount=containers.TotalCount,
				Items= lstContainerResult
			};
			return pagedlistResult;
		}
		public async Task<SearchItem> SearchItem()
		{
			var lst = await _shipmentBookingDataProvider.ListAsync();
			List<Booking> bookingModels = lst.Items;
			var DestinationPort = bookingModels.Select(x => x.PortOfDelivery).Distinct();
			var OriginPorts = bookingModels.Select(x => x.PortOfLoading).Distinct();
			var Carriers = bookingModels.Select(x => x.Carrier).Distinct();
			SearchItem getSearchItemDTO = new SearchItem()
			{
				DestPorts = DestinationPort,
				OriginPorts = OriginPorts,
				Carriers = Carriers,
			};
			return getSearchItemDTO;
		}
		


	}
}
