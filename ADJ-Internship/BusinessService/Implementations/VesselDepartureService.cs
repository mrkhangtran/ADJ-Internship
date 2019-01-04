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

		private readonly IArriveOfDespatchRepository _arriveOfDespatchRepository;
		private readonly IDataProvider<ArriveOfDespatch> _arriveOfDespatchDataProvider;


		public VesselDepartureService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
			IDataProvider<Manifest> manifestDataProvider, IManifestRepository manifestRepository,
			IDataProvider<ArriveOfDespatch> arriveOfDespatchDataProvider, IArriveOfDespatchRepository arriveOfDespatchRepository,
			IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository shipmentBookingRepository, IContainerRepository containerRepository,
			IDataProvider<Container> containerDataProvider) : base(unitOfWork, mapper, appContext)
		{
			this._manifestDataProvider = manifestDataProvider;
			this._manifestRepository = manifestRepository;

			this._shipmentBookingDataProvider = bookingDataProvider;
			this._shipmentBookingRepository = shipmentBookingRepository;

			this._containerRepository = containerRepository;
			this._containerDataProvider = containerDataProvider;

			this._arriveOfDespatchDataProvider = arriveOfDespatchDataProvider;
			this._arriveOfDespatchRepository = arriveOfDespatchRepository;

		}

		public VesselDepartureDtos Achive(VesselDepartureDtos model)
		{
			for (int i = 0; i < model.lstContainerDto.Count; i++)
			{
				if (model.lstContainerDto[i].checkClick == true)
				{
					if (model.lstContainerDto[i].originPortChange != model.lstContainerDto[i].OriginPort)
					{
						foreach(var n in model.lstArriveOfDespatchDto)
						{
							if (n.ContainerId == model.lstContainerDto[i].Id)
							{
								model.lstArriveOfDespatchDto[i].OriginPort = model.lstContainerDto[i].originPortChange;
							}
						}
						model.lstContainerDto[i].Status = ContainerStatus.Despatch;
					}
					if (model.lstContainerDto[i].destPortChange != model.lstContainerDto[i].DestPort)
					{
						foreach (var n in model.lstArriveOfDespatchDto)
						{
							if (n.ContainerId == model.lstContainerDto[i].Id)
							{
								model.lstArriveOfDespatchDto[i].DestinationPort = model.lstContainerDto[i].destPortChange;
							}
						}
						model.lstContainerDto[i].Status = ContainerStatus.Despatch;
					}
					if (model.lstContainerDto[i].modeChange != model.lstContainerDto[i].Loading)
					{
						foreach (var n in model.lstArriveOfDespatchDto)
						{
							if (n.ContainerId == model.lstContainerDto[i].Id)
							{
								model.lstArriveOfDespatchDto[i].Mode = model.lstContainerDto[i].modeChange;
							}
						}
						model.lstContainerDto[i].Loading = model.lstContainerDto[i].modeChange;
						model.lstContainerDto[i].Status = ContainerStatus.Despatch;
					}
					if (model.lstContainerDto[i].carrierChange != model.lstContainerDto[i].Carrier)
					{
						foreach (var n in model.lstArriveOfDespatchDto)
						{
							if (n.ContainerId == model.lstContainerDto[i].Id)
							{
								model.lstArriveOfDespatchDto[i].Carrier = model.lstContainerDto[i].carrierChange;
							}
						}		
						model.lstContainerDto[i].Status = ContainerStatus.Despatch;
					}
				}
			}

			//Group by service
			model.lstContainerDto = model.lstContainerDto.OrderBy(p => p.OriginPort).OrderBy(m => m.DestPort).OrderBy(m => m.Loading).OrderBy(m => m.Carrier).ToList();


			return model;
		}

		public async Task<VesselDepartureDtos> ListContainerDtoAsync(int? containerId, string origin = null, string originPort = null, string status = null, int? pageIndex = null, int? pageSize = null, DateTime? etdFrom = null, DateTime? etdTo = null)
		{
			//sort string
			string sortStr = "Status ASC";
			VesselDepartureDtos filterResult = new VesselDepartureDtos();
			var manifests = await _manifestDataProvider.ListAsync();
			var bookings = await _shipmentBookingDataProvider.ListAsync();
			var arriveOfDespatchs = await _arriveOfDespatchDataProvider.ListAsync();

			List<ManifestDto> lstManifestDto = Mapper.Map<List<ManifestDto>>(manifests.Items);
			List<BookingDto> lstBookingDto = Mapper.Map<List<BookingDto>>(bookings.Items);
			List<ArriveOfDespatchDto> lstArriveOfDespatchDto = Mapper.Map<List<ArriveOfDespatchDto>>(arriveOfDespatchs.Items);

			//if status = null, filter both Pendding and Despatch and list result is from 2 list
			if (status == null)
			{
				Expression<Func<Container, bool>> filterPending = x => x.Id > 0;
				//status= Pending
				{
					Expression<Func<Container, bool>> filterStatus = x => x.Status.ToString() == ContainerStatus.Pending.ToString();
					filterPending = filterPending.And(filterStatus);

					if (origin != null)
					{
						Expression<Func<Container, bool>> filterOrigin = x => x.Manifests.Where(p => p.Booking.Order.Origin == origin).Count() > 0;
						filterPending = filterPending.And(filterOrigin);
					}
					if (originPort != null)
					{
						Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.PortOfLoading == originPort).Count() > 0;
						filterPending = filterPending.And(filterOriginPort);
					}
					if (containerId != null)
					{
						Expression<Func<Container, bool>> filterContainerId = x => x.Id == containerId;
						filterPending = filterPending.And(filterContainerId);
					}
					if (etdFrom != null)
					{
						Expression<Func<Container, bool>> filterETDFrom = x => x.Manifests.Where(p => p.Booking.ETD >= etdFrom).Count() > 0;
						filterPending = filterPending.And(filterETDFrom);
					}
					if (etdTo != null)
					{
						Expression<Func<Container, bool>> filterETDTo = x => x.Manifests.Where(p => p.Booking.ETD <= etdTo).Count() > 0;
						filterPending = filterPending.And(filterETDTo);
					}
				}
				//get list result 
				var lstPending = await _containerDataProvider.ListAsync(filterPending, sortStr, true, pageIndex, pageSize);

				//status = Despatch
				Expression<Func<Container, bool>> filterDespatch = x => x.Id > 0;

				{
					Expression<Func<Container, bool>> filterStatus = x => x.Status.ToString() == ContainerStatus.Despatch.ToString();
					filterDespatch = filterDespatch.And(filterStatus);

					if (origin != null)
					{
						Expression<Func<Container, bool>> filterOrigin = x => x.Manifests.Where(p => p.Booking.Order.Origin == origin).Count() > 0;
						filterDespatch = filterDespatch.And(filterOrigin);
					}
					if (originPort != null)
					{
						Expression<Func<Container, bool>> filterOriginPort = x => x.ArriveOfDespatch.OriginPort == originPort;
						filterDespatch = filterDespatch.And(filterOriginPort);
					}
					if (containerId != null)
					{
						Expression<Func<Container, bool>> filterContainerId = x => x.Id == containerId;
						filterDespatch = filterDespatch.And(filterContainerId);
					}
					if (etdFrom != null)
					{
						Expression<Func<Container, bool>> filterETDFrom = x => x.ArriveOfDespatch.ETD >= etdFrom;
						filterDespatch = filterDespatch.And(filterETDFrom);
					}
					if (etdTo != null)
					{
						Expression<Func<Container, bool>> filterETDTo = x => x.ArriveOfDespatch.ETD <= etdTo;
						filterDespatch = filterDespatch.And(filterETDTo);
					}
				}

				var lstDespatch = await _containerDataProvider.ListAsync(filterDespatch, sortStr, true, pageIndex, pageSize);

				for (int item = 0; item < lstDespatch.Items.Count; item++)
				{
					lstPending.Items.Add(lstDespatch.Items[item]);
				}
				//get list result when  status = null
				filterResult.lstContainerDto = Mapper.Map<List<ContainerDto>>(lstPending.Items);

				for (int containerIndex = 0; containerIndex < filterResult.lstContainerDto.Count; containerIndex++)
				{
					if (filterResult.lstContainerDto[containerIndex].Status == ContainerStatus.Pending)
					{
						foreach (var booking in lstBookingDto)
						{
							foreach (var manifest in lstManifestDto)
							{
								if (manifest.ContainerId == filterResult.lstContainerDto[containerIndex].Id && booking.Id == manifest.BookingId)
								{
									filterResult.lstContainerDto[containerIndex].OriginPort = booking.PortOfLoading;
									filterResult.lstContainerDto[containerIndex].DestPort = booking.PortOfDelivery;
									filterResult.lstContainerDto[containerIndex].Loading = booking.Mode;
									filterResult.lstContainerDto[containerIndex].Carrier = booking.Carrier;
									filterResult.lstContainerDto[containerIndex].ETD = booking.ETD;
									filterResult.lstContainerDto[containerIndex].ETA = booking.ETA;
								}
							}
						}
					} //end pending
					else if (filterResult.lstContainerDto[containerIndex].Status == ContainerStatus.Despatch)
					{

						foreach (var arriveOfDespatch in lstArriveOfDespatchDto)
						{
							if (arriveOfDespatch.ContainerId == filterResult.lstContainerDto[containerIndex].Id)
							{
								filterResult.lstContainerDto[containerIndex].OriginPort = arriveOfDespatch.OriginPort;
								filterResult.lstContainerDto[containerIndex].DestPort = arriveOfDespatch.DestinationPort;
								filterResult.lstContainerDto[containerIndex].Loading = arriveOfDespatch.Mode;
								filterResult.lstContainerDto[containerIndex].Carrier = arriveOfDespatch.Carrier;
								filterResult.lstContainerDto[containerIndex].ETD = arriveOfDespatch.ETD;
								filterResult.lstContainerDto[containerIndex].ETA = arriveOfDespatch.ETA;
							}
						}

					}
				}
				//Group by service
				filterResult.lstContainerDto = filterResult.lstContainerDto.OrderBy(p => p.OriginPort).OrderBy(m => m.DestPort).OrderBy(m => m.Loading).OrderBy(m => m.Carrier).ToList();
				return filterResult;
			} //end if(status == null)

			else
			{
				if (status == ContainerStatus.Pending.ToString())           //status= Pending
				{
					Expression<Func<Container, bool>> filterPending = x => x.Id > 0;

					Expression<Func<Container, bool>> filterStatus = x => x.Status.ToString() == ContainerStatus.Pending.ToString();
					filterPending = filterPending.And(filterStatus);

					if (origin != null)
					{
						Expression<Func<Container, bool>> filterOrigin = x => x.Manifests.Where(p => p.Booking.Order.Origin == origin).Count() > 0;
						filterPending = filterPending.And(filterOrigin);
					}
					if (originPort != null)
					{
						Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.PortOfLoading == originPort).Count() > 0;
						filterPending = filterPending.And(filterOriginPort);
					}
					if (containerId != null)
					{
						Expression<Func<Container, bool>> filterContainerId = x => x.Id == containerId;
						filterPending = filterPending.And(filterContainerId);
					}
					if (etdFrom != null)
					{
						Expression<Func<Container, bool>> filterETDFrom = x => x.Manifests.Where(p => p.Booking.ETD >= etdFrom).Count() > 0;
						filterPending = filterPending.And(filterETDFrom);
					}
					if (etdTo != null)
					{
						Expression<Func<Container, bool>> filterETDTo = x => x.Manifests.Where(p => p.Booking.ETD <= etdTo).Count() > 0;
						filterPending = filterPending.And(filterETDTo);
					}

					//get list result 
					var lstPending = await _containerDataProvider.ListAsync(filterPending, sortStr, true, pageIndex, pageSize);

					filterResult.lstContainerDto = Mapper.Map<List<ContainerDto>>(lstPending.Items);

					for (int containerIndex = 0; containerIndex < filterResult.lstContainerDto.Count; containerIndex++)
					{
						foreach (var booking in lstBookingDto)
						{
							foreach (var manifest in lstManifestDto)
							{
								if (manifest.ContainerId == filterResult.lstContainerDto[containerIndex].Id && booking.Id == manifest.BookingId)
								{
									filterResult.lstContainerDto[containerIndex].OriginPort = booking.PortOfLoading;
									filterResult.lstContainerDto[containerIndex].DestPort = booking.PortOfDelivery;
									filterResult.lstContainerDto[containerIndex].Loading = booking.Mode;
									filterResult.lstContainerDto[containerIndex].Carrier = booking.Carrier;
									filterResult.lstContainerDto[containerIndex].ETD = booking.ETD;
									filterResult.lstContainerDto[containerIndex].ETA = booking.ETA;
								}
							}
						}
						//end pending
					}

				}

				if (status == ContainerStatus.Despatch.ToString()) //status = Despatch
				{
					Expression<Func<Container, bool>> filterDespatch = x => x.Id > 0;

					Expression<Func<Container, bool>> filterStatus = x => x.Status.ToString() == ContainerStatus.Despatch.ToString();
					filterDespatch = filterDespatch.And(filterStatus);

					if (origin != null)
					{
						Expression<Func<Container, bool>> filterOrigin = x => x.Manifests.Where(p => p.Booking.Order.Origin == origin).Count() > 0;
						filterDespatch = filterDespatch.And(filterOrigin);
					}
					if (originPort != null)
					{
						Expression<Func<Container, bool>> filterOriginPort = x => x.ArriveOfDespatch.OriginPort == originPort;
						filterDespatch = filterDespatch.And(filterOriginPort);
					}
					if (containerId != null)
					{
						Expression<Func<Container, bool>> filterContainerId = x => x.Id == containerId;
						filterDespatch = filterDespatch.And(filterContainerId);
					}
					if (etdFrom != null)
					{
						Expression<Func<Container, bool>> filterETDFrom = x => x.ArriveOfDespatch.ETD >= etdFrom;
						filterDespatch = filterDespatch.And(filterETDFrom);
					}
					if (etdTo != null)
					{
						Expression<Func<Container, bool>> filterETDTo = x => x.ArriveOfDespatch.ETD <= etdTo;
						filterDespatch = filterDespatch.And(filterETDTo);
					}

					//get list result with status = Despatch
					var lstDespatch = await _containerDataProvider.ListAsync(filterDespatch, sortStr, true, pageIndex, pageSize);

					filterResult.lstContainerDto = Mapper.Map<List<ContainerDto>>(lstDespatch.Items);

					for (int containerIndex = 0; containerIndex < filterResult.lstContainerDto.Count; containerIndex++)
					{
						foreach (var arriveOfDespatch in lstArriveOfDespatchDto)
						{
							if (arriveOfDespatch.ContainerId == filterResult.lstContainerDto[containerIndex].Id)
							{
								filterResult.lstContainerDto[containerIndex].OriginPort = arriveOfDespatch.OriginPort;
								filterResult.lstContainerDto[containerIndex].DestPort = arriveOfDespatch.DestinationPort;
								filterResult.lstContainerDto[containerIndex].Loading = arriveOfDespatch.Mode;
								filterResult.lstContainerDto[containerIndex].Carrier = arriveOfDespatch.Carrier;
								filterResult.lstContainerDto[containerIndex].ETD = arriveOfDespatch.ETD;
								filterResult.lstContainerDto[containerIndex].ETA = arriveOfDespatch.ETA;
							}
						}
					}
				}

				//Group by service
				filterResult.lstContainerDto = filterResult.lstContainerDto.OrderBy(p => p.OriginPort).OrderBy(m => m.DestPort).OrderBy(m => m.Loading).OrderBy(m => m.Carrier).ToList();

				return filterResult;

			}
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
