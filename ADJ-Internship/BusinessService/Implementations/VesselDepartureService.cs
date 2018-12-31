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

		public VesselDepartureService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Manifest> manifestDataProvider, IManifestRepository manifestRepository, IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository shipmentBookingRepository, IContainerRepository containerRepository, IDataProvider<Container> containerDataProvider, IOrderDetailRepository orderDetailRepository, IDataProvider<OrderDetail> orderDetailDataProvider) : base(unitOfWork, mapper, appContext)
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
		public async Task<PagedListResult<ContainerDto>> ListContainerAsync(int pageIndex = 1, int pageSize = 2, string Origin = null, string OriginPort = null, string Loading = null, DateTime? ETDFrom = null, DateTime? ETDTo = null, string Status = null, string Vendor = null, string PONumber = null, string Item = null, string Warehouse = null)
		{
			Expression<Func<Container, bool>> All = x => x.Id > 0;
			if (Origin != null)
			{
				Expression<Func<Container, bool>> filterDestinationPort = x => x.Manifests.Where(p => p.Booking.Order.Origin == Origin).Count() > 0;
				All = All.And(filterDestinationPort);
			}
			if (OriginPort != null)
			{
				Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.PortOfLoading == OriginPort).Count() > 0;
				All = All.And(filterOriginPort);
			}
			if (Loading != null)
			{
				Expression<Func<Container, bool>> filterCarrier = x => x.Manifests.Where(p => p.Booking.Carrier == Loading).Count() > 0;
				All = All.And(filterCarrier);
			}
			if (ETDFrom != null)
			{
				Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.ETD.CompareTo(ETDFrom) > 0).Count() > 0;
				All = All.And(filterOriginPort);
			}
			if (ETDTo != null)
			{
				Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.ETD.CompareTo(ETDTo) < 0).Count() > 0;
				All = All.And(filterOriginPort);
			}
			if (Status != null)
			{
				Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.Status.ToString() == Status).Count() > 0;
				All = All.And(filterOriginPort);
			}
			if (PONumber != null)
			{
				Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.PONumber == PONumber).Count() > 0;
				All = All.And(filterOriginPort);
			}
			if (Item != null)
			{
				Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.ItemNumber == Item).Count() > 0;
				All = All.And(filterOriginPort);
			}

			//string sort
			string sortStr="Status ASC";
			var containers = await _containerDataProvider.ListAsync(All, sortStr, true, pageIndex, pageSize);

			var pagedResult = new PagedListResult<ContainerDto>
			{
				TotalCount = containers.TotalCount,
				PageCount = containers.PageCount,
				CurrentFilter = null,
				Items = Mapper.Map<List<ContainerDto>>(containers.Items)
			};

			return pagedResult;
		}
		public async Task<SearchItem> SearchItem()
		{
			var lst = await _shipmentBookingDataProvider.ListAsync();
			List<Booking> bookingModels = lst.Items;
			var DestinationPort = bookingModels.Select(x => x.PortOfDelivery).Distinct();
			var OriginPorts = bookingModels.Select(x => x.PortOfLoading).Distinct();
			var Carriers = bookingModels.Select(x => x.Carrier).Distinct();
			var PortOfDeliverys = bookingModels.Select(x => x.PortOfDelivery).Distinct();
			List<string> status = Enum.GetNames(typeof(OrderStatus)).ToList();
			SearchItem getSearchItemDTO = new SearchItem()
			{
				OriginPorts = OriginPorts,
				Carriers = Carriers,
				Dest= PortOfDeliverys,
				
			};
			return getSearchItemDTO;
		}
		public async Task<ShipmentManifestsDtos> CreateOrUpdateContainerAsync(ShipmentManifestsDtos rq)
		{
			Manifest entity = new Manifest();
			Container container = new Container();
			if (rq.Id > 0)
			{
				container = await _containerRepository.GetByIdAsync(rq.Id, true);
				container.Name = rq.Name;
				container.Size = rq.Size;
				container.Loading = rq.Loading;
				container.PackType = rq.PackType;
				_containerRepository.Update(container);
				foreach (var itemManifest in rq.Manifests)
				{
					var manifest = await _manifestRepository.Query(p => p.Id == itemManifest.Id, true).SelectAsync();
					entity = manifest[0];
					if (entity == null)
					{
						throw new AppException("Shipment Manifest Not Found");
					}
					else
					{
						entity.Quantity = itemManifest.ShipQuantity;
						if (itemManifest.BookingQuantity - itemManifest.ShipQuantity == 0)
						{
							entity.Booking.Status = OrderStatus.Manifested;
						}
						if (itemManifest.BookingQuantity - itemManifest.ShipQuantity > 0)
						{
							entity.Booking.Status = OrderStatus.PartlyManifested;
						}
					}
					_shipmentBookingRepository.Update(entity.Booking);
					_manifestRepository.Update(entity);
				}
			}
			else
			{
				container.Name = rq.Name;
				container.Size = rq.Size;
				container.Loading = rq.Loading;
				container.PackType = rq.PackType;
				//container.Status = OrderStatus.Pending;
				List<Manifest> manifests = new List<Manifest>();
				foreach (var item in rq.Manifests)
				{
					if (item.selectedItem == true)
					{
						var orderDeatail = await _orderDetailRepository.Query(p => p.ItemNumber == item.ItemNumber, false).SelectAsync();
						var booking = await _shipmentBookingRepository.Query(p => p.Id == item.BookingId, false).SelectAsync();
						entity.Quantity = item.ShipQuantity;
						entity.Container = container;
						entity.Loading = container.Loading;
						entity.Loading = container.Loading;
						entity.PackType = container.PackType;
						entity.Size = container.Size;
						entity.BookingId = item.BookingId;
						entity.Cartons = orderDeatail[0].Cartons;
						entity.Cube = orderDeatail[0].Cube;
						entity.FreightTerms = item.FreightTerms;
						entity.KGS = orderDeatail[0].KGS;
						if (item.BookingQuantity - item.ShipQuantity == 0)
						{
							booking[0].Status = OrderStatus.Manifested;
						}
						if (item.BookingQuantity - item.ShipQuantity > 0)
						{
							booking[0].Status = OrderStatus.PartlyManifested;
						}
						entity.Booking = booking[0];

						manifests.Add(entity);
						_shipmentBookingRepository.Update(booking[0]);
					}
				}
				if (manifests.Count > 0)
				{
					container.Manifests = manifests;
					_containerRepository.Insert(container);
				}
			}
			await UnitOfWork.SaveChangesAsync();
			var rs = Mapper.Map<ShipmentManifestsDtos>(container);
			return rs;
		}


	}
}
