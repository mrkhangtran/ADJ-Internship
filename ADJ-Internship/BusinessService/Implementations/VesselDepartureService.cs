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


		public VesselDepartureService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, 
			IDataProvider<Manifest> manifestDataProvider, IManifestRepository manifestRepository, 
			IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository shipmentBookingRepository, IContainerRepository containerRepository, 
			IDataProvider<Container> containerDataProvider) : base(unitOfWork, mapper, appContext)
		{
			this._manifestDataProvider = manifestDataProvider;
			this._manifestRepository = manifestRepository;

			this._shipmentBookingDataProvider = bookingDataProvider;
			this._shipmentBookingRepository = shipmentBookingRepository;

			this._containerRepository = containerRepository;
			this._containerDataProvider = containerDataProvider;

		}

		public VesselDepartureDtos Achive(VesselDepartureDtos model)
		{
			for (int i = 0; i < model.lstContainerDto.Count; i++)
			{
				if (model.lstContainerDto[i].checkClick == true)
				{
					if (model.lstContainerDto[i].originPortChange != model.lstContainerDto[i].OriginPort)
					{
						model.lstContainerDto[i].OriginPort = model.lstContainerDto[i].originPortChange;
						model.lstContainerDto[i].Status = ContainerStatus.Pending;
					}
					if (model.lstContainerDto[i].destPortChange != model.lstContainerDto[i].DestPort)
					{
						model.lstContainerDto[i].DestPort = model.lstContainerDto[i].destPortChange;
						model.lstContainerDto[i].Status = ContainerStatus.Pending;
					}
					if (model.lstContainerDto[i].modeChange != model.lstContainerDto[i].Loading)
					{
						model.lstContainerDto[i].Loading = model.lstContainerDto[i].modeChange;
						model.lstContainerDto[i].Status = ContainerStatus.Pending;
					}
					if (model.lstContainerDto[i].carrierChange != model.lstContainerDto[i].Carrier)
					{
						model.lstContainerDto[i].Carrier = model.lstContainerDto[i].carrierChange;
						model.lstContainerDto[i].Status = ContainerStatus.Pending;
					}
				}

			}
			model.lstContainerDto = model.lstContainerDto.OrderBy(p => p.OriginPort).OrderBy(m => m.DestPort).OrderBy(m => m.Loading).OrderBy(m => m.Carrier).ToList();


			return model;
		}

		public async Task<VesselDepartureDtos> ListContainerDtoAsync(int? pageIndex, int? pageSize, string origin = null, string originPort = null, string container = null, string status = null, DateTime? etdFrom = null, DateTime? etdTo = null)
		{
			
			Expression<Func<Container, bool>> All = x => x.Id > 0;

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

			var manifests = await _manifestDataProvider.ListAsync();
			var bookings = await _shipmentBookingDataProvider.ListAsync();

			VesselDepartureDtos  filterResult = new VesselDepartureDtos();
			filterResult.lstContainerDto = Mapper.Map<List<ContainerDto>>(containers.Items);
			filterResult.lstContainerDto= filterResult.lstContainerDto.OrderBy(p => p.OriginPort).OrderBy(m => m.DestPort).OrderBy(m => m.Loading).OrderBy(m => m.Carrier).ToList();

			for(int i = 0; i < filterResult.lstContainerDto.Count; i++)
			{
				foreach(var booking in bookings.Items)
				{
					foreach (var manifest in manifests.Items)
					{
						if (booking.Id == manifest.BookingId && manifest.ContainerId == filterResult.lstContainerDto[i].Id) 
						{
							filterResult.lstContainerDto[i].ETA = booking.ETA;
							filterResult.lstContainerDto[i].ETD = booking.ETD;
						}
					}
				}
				
				
			}

			return filterResult;
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
