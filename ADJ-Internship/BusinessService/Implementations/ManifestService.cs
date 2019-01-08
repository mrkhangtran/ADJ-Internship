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
  public class ManifestService : ServiceBase, IManifestService
  {

    private readonly IManifestRepository _manifestRepository;
    private readonly IDataProvider<Manifest> _manifestDataProvider;

    private readonly IShipmentBookingRepository _shipmentBookingRepository;
    private readonly IDataProvider<Booking> _shipmentBookingDataProvider;

    private readonly IContainerRepository _containerRepository;
    private readonly IDataProvider<Container> _containerDataProvider;

    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IDataProvider<OrderDetail> _orderDetailDataProvider;

    public ManifestService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Manifest> manifestDataProvider, IManifestRepository manifestRepository, IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository shipmentBookingRepository, IContainerRepository containerRepository, IDataProvider<Container> containerDataProvider, IOrderDetailRepository orderDetailRepository, IDataProvider<OrderDetail> orderDetailDataProvider) : base(unitOfWork, mapper, appContext)
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
    public async Task<PagedListResult<ShipmentManifestsDtos>> ListManifestDtoAsync(int pageIndex = 1, int pageSize = 2, string DestinationPort = null, string OriginPort = null, string Carrier = null, DateTime? ETDFrom = null, DateTime? ETDTo = null, string Status = null, string Vendor = null, string PONumber = null, string Item = null)
    {
      PagedListResult<ShipmentManifestsDtos> pageResult = new PagedListResult<ShipmentManifestsDtos>();
      List<ShipmentManifestsDtos> shipmentManifestsDtos = new List<ShipmentManifestsDtos>();
      List<ShipmentManifestsDtos> currentPage = new List<ShipmentManifestsDtos>();
      List<ShipmentManifestsDtos> previousPage = new List<ShipmentManifestsDtos>();
      Expression<Func<Container, bool>> All = x => x.Id > 0;
      Expression<Func<Booking, bool>> AllBooking = o => o.Id > 0;
      if (DestinationPort != null)
      {
        Expression<Func<Container, bool>> filterDestinationPort = x => x.Manifests.Where(p => p.Booking.PortOfDelivery == DestinationPort).Count() > 0;
        All = All.And(filterDestinationPort);
      }
      if (OriginPort != null)
      {
        Expression<Func<Container, bool>> filterOriginPort = x => x.Manifests.Where(p => p.Booking.PortOfLoading == OriginPort).Count() > 0;
        All = All.And(filterOriginPort);
      }
      if (Carrier != null)
      {
        Expression<Func<Container, bool>> filterCarrier = x => x.Manifests.Where(p => p.Booking.Carrier == Carrier).Count() > 0;
        All = All.And(filterCarrier);
      }
      if (ETDFrom != null)
      {
        Expression<Func<Container, bool>> filterETDFrom = x => x.Manifests.Where(p => p.Booking.ETD.CompareTo(ETDFrom) > 0).Count() > 0;
        Expression<Func<Booking, bool>> filter = x => x.ETD.CompareTo(ETDFrom) > 0;
        AllBooking = AllBooking.And(filter);
        All = All.And(filterETDFrom);
      }
      if (ETDTo != null)
      {
        Expression<Func<Container, bool>> filterETDTo = x => x.Manifests.Where(p => p.Booking.ETD.CompareTo(ETDTo) < 0).Count() > 0;
        Expression<Func<Booking, bool>> filter = x => x.ETD.CompareTo(ETDTo) > 0;
        AllBooking = AllBooking.And(filter);
        All = All.And(filterETDTo);
      }
      if (Status != null)
      {
        Expression<Func<Container, bool>> filterStatus = x => x.Manifests.Where(p => p.Booking.Status.ToString() == Status).Count() > 0;
        Expression<Func<Booking, bool>> filter = x => x.Status.ToString() == Status;
        AllBooking = AllBooking.And(filter);
        All = All.And(filterStatus);
      }
      if (Vendor != null)
      {
        Expression<Func<Container, bool>> filterVendor = x => x.Manifests.Where(p => p.Booking.Order.Vendor == Vendor).Count() > 0;
        Expression<Func<Booking, bool>> filter = x => x.ETD.CompareTo(ETDFrom) > 0;
        AllBooking = AllBooking.And(filter);
        All = All.And(filterVendor);
      }
      if (PONumber != null)
      {
        Expression<Func<Container, bool>> filterPONumber = x => x.Manifests.Where(p => p.Booking.PONumber == PONumber).Count() > 0;
        Expression<Func<Booking, bool>> filter = x => x.PONumber == PONumber;
        AllBooking = AllBooking.And(filter);
        All = All.And(filterPONumber);
      }
      if (Item != null)
      {
        Expression<Func<Container, bool>> filterItem = x => x.Manifests.Where(p => p.Booking.ItemNumber == Item).Count() > 0;
        Expression<Func<Booking, bool>> filter = x => x.ItemNumber == Item;
        AllBooking = AllBooking.And(filter);
        All = All.And(filterItem);
      }
      Expression<Func<Booking, bool>> filterBooking = p => p.PortOfLoading == OriginPort && p.PortOfDelivery == DestinationPort && p.Carrier == Carrier;
      var containers = await _containerDataProvider.ListAsync(All, null, true, pageIndex, pageSize);
      AllBooking = AllBooking.And(filterBooking);
      var bookings = await _shipmentBookingDataProvider.ListAsync(AllBooking, null, true);
      var previous = await _containerDataProvider.ListAsync(All, null, true, pageIndex, pageSize);

      List<Booking> listBooking = bookings.Items;
      List<Booking> listBookingNoContainer = listBooking.Where(p => p.Status == OrderStatus.BookingMade || p.Status == OrderStatus.PartlyManifested).OrderBy(p => p.PortOfLoading).ToList();
      bool checkEmptyContainer = false;
      if (listBookingNoContainer.Count() == 0)
      {
        checkEmptyContainer = true;
      }
      if (checkEmptyContainer == false && pageIndex != 1)
      {
        previous = await _containerDataProvider.ListAsync(All, null, true, pageIndex - 1, pageSize);
      }
      //currentDTO
      foreach (var container in containers.Items)
      {
        List<ItemManifest> itemManifests = new List<ItemManifest>();
        ShipmentManifestsDtos shipmentManifestsDto = new ShipmentManifestsDtos();
        foreach (var item in container.Manifests)
        {
          decimal temp = 0;
          foreach (var manifest in item.Booking.Manifests)
          {
            temp += manifest.Quantity;
          }
          var orderDeatail = await _orderDetailRepository.Query(p => p.ItemNumber == item.Booking.ItemNumber, true).SelectAsync();
          ItemManifest itemManifest = new ItemManifest()
          {
            Id = item.Id,
            Vendor = orderDeatail[0].Order.Vendor,
            Carrier = item.Booking.Carrier,
            PONumber = item.Booking.PONumber,
            ItemNumber = item.Booking.ItemNumber,
            ETDDate = item.Booking.ETD,
            BookingQuantity = item.Booking.Quantity,
            OpenQuantity = item.Booking.Quantity - temp,
            ShipQuantity = item.Quantity,
            BookingCartons = item.Booking.Quantity * (decimal)item.Booking.Cartons,
            ShipCartons = item.Quantity * (decimal)item.Cartons,
            BookingCube = item.Booking.Quantity * (decimal)item.Booking.Cube,
            ShipCube = item.Quantity * (decimal)item.Cube,
            NetWeight = item.Quantity * (decimal)item.KGS,
            Manifested = item.Booking.Status.ToString(),
          };
          itemManifests.Add(itemManifest);
        }
        shipmentManifestsDto.Id = container.Id;
        shipmentManifestsDto.Manifests = itemManifests;
        shipmentManifestsDto.Name = container.Name;
        shipmentManifestsDto.Size = container.Size;
        shipmentManifestsDto.Loading = container.Loading;
        shipmentManifestsDto.PackType = container.PackType;
        currentPage.Add(shipmentManifestsDto);
      }
      if (containers.TotalCount % 2 == 0 && checkEmptyContainer == false)
      {
        pageResult.PageCount = containers.PageCount + 1;
      }
      else
        pageResult.PageCount = containers.PageCount;
      if (checkEmptyContainer == true)
      {
        pageResult.Items = currentPage;
        pageResult.PageCount = containers.PageCount;
      }
      ShipmentManifestsDtos shipmentNoContainerDto = new ShipmentManifestsDtos();// Define DTO No Container
      List<ItemManifest> itemNoContainer = new List<ItemManifest>(); // Define List Booking No Manifest Container Or  Status is PartlyManifest
      if (checkEmptyContainer == false)
      {
        foreach (var bookingNoContainer in listBookingNoContainer)
        {
          var orderDeatail = await _orderDetailRepository.Query(p => p.ItemNumber == bookingNoContainer.ItemNumber, true).SelectAsync();
          if (bookingNoContainer.Status == OrderStatus.BookingMade)
          {
            ItemManifest itemManifest = new ItemManifest()
            {
              Id = 0,
              Vendor = orderDeatail[0].Order.Vendor,
              Carrier = bookingNoContainer.Carrier,
              PONumber = bookingNoContainer.PONumber,
              ItemNumber = bookingNoContainer.ItemNumber,
              ETDDate = bookingNoContainer.ETD,
              BookingQuantity = bookingNoContainer.Quantity,
              BookingCartons = bookingNoContainer.Quantity * (decimal)bookingNoContainer.Cartons,
              OpenQuantity = bookingNoContainer.Quantity,
              BookingCube = bookingNoContainer.Quantity * (decimal)bookingNoContainer.Cube,
              Manifested = bookingNoContainer.Status.ToString(),
              BookingId = bookingNoContainer.Id,
              Cube=bookingNoContainer.Cube,
              Carton=bookingNoContainer.Cartons,
              KGS=orderDeatail[0].KGS,
            };
            itemNoContainer.Add(itemManifest);
          }
          if (bookingNoContainer.Status == OrderStatus.PartlyManifested)
          {
            ItemManifest itemManifest = new ItemManifest()
            {
              Id = 0,
              Vendor = orderDeatail[0].Order.Vendor,
              Carrier = bookingNoContainer.Carrier,
              PONumber = bookingNoContainer.PONumber,
              ItemNumber = bookingNoContainer.ItemNumber,
              ETDDate = bookingNoContainer.ETD,
              BookingQuantity = bookingNoContainer.Quantity,
              BookingCartons = bookingNoContainer.Quantity * (decimal)bookingNoContainer.Cartons,
              BookingCube = bookingNoContainer.Quantity * (decimal)bookingNoContainer.Cube,
              Manifested = bookingNoContainer.Status.ToString(),
              BookingId = bookingNoContainer.Id,
              Cube = bookingNoContainer.Cube,
              Carton = bookingNoContainer.Cartons,
              KGS = orderDeatail[0].KGS,
            };
            decimal temp = 0;
            foreach (var quantity in bookingNoContainer.Manifests)
            {
              temp += quantity.Quantity;
            }
            itemManifest.OpenQuantity = bookingNoContainer.Quantity - temp;
            itemNoContainer.Add(itemManifest);
          }
        }
        shipmentNoContainerDto.Id = 0;
        shipmentNoContainerDto.Manifests = itemNoContainer;//Dto no container

        foreach (var next in previous.Items) //get DTO previous page 
        {
          List<ItemManifest> itemManifests = new List<ItemManifest>();
          ShipmentManifestsDtos shipmentManifestsNextDto = new ShipmentManifestsDtos();
          foreach (var item in next.Manifests)
          {
            decimal temp = 0;
            foreach (var manifest in item.Booking.Manifests)
            {
              temp += manifest.Quantity;
            }
            ItemManifest itemManifest = new ItemManifest()
            {
              Id = item.Id,
              Vendor = item.Booking.Factory,
              Carrier = item.Booking.Carrier,
              PONumber = item.Booking.PONumber,
              ItemNumber = item.Booking.ItemNumber,
              ETDDate = item.Booking.ETD,
              BookingQuantity = item.Booking.Quantity,
              OpenQuantity = item.Booking.Quantity - temp,
              ShipQuantity = item.Quantity,
              BookingCartons = item.Booking.Quantity * (decimal)item.Booking.Cartons,
              ShipCartons = item.Quantity * (decimal)item.Cartons,
              BookingCube = item.Booking.Quantity * (decimal)item.Booking.Cube,
              ShipCube = item.Quantity * (decimal)item.Cube,
              NetWeight = item.Quantity * (decimal)item.KGS,
              Manifested = item.Booking.Status.ToString(),
            };
            itemManifests.Add(itemManifest);
          }
          shipmentManifestsNextDto.Id = next.Id;
          shipmentManifestsNextDto.Manifests = itemManifests;
          shipmentManifestsNextDto.Name = next.Name;
          shipmentManifestsNextDto.Size = next.Size;
          shipmentManifestsNextDto.Loading = next.Loading;
          shipmentManifestsNextDto.PackType = next.PackType;
          previousPage.Add(shipmentManifestsNextDto);
        }
        if (pageIndex == 1)
        {
          shipmentManifestsDtos.Add(shipmentNoContainerDto);
          if (currentPage.Count() > 0)
          {
            shipmentManifestsDtos.Add(currentPage[0]);
          }
          pageResult.Items = shipmentManifestsDtos;
        }
        if (pageIndex > 1)
        {
          shipmentManifestsDtos.Add(previousPage[1]);
          if (currentPage.Count>0)
          {
            shipmentManifestsDtos.Add(currentPage[0]);
          }
          pageResult.Items = shipmentManifestsDtos;
        }
      }
      return pageResult;
    }
    public async Task<SearchingManifestItem> SearchItem()
    {
      var lst = await _shipmentBookingDataProvider.ListAsync();
      List<Booking> bookingModels = lst.Items;
      var DestinationPort = bookingModels.Select(x => x.PortOfDelivery).Distinct();
      var OriginPorts = bookingModels.Select(x => x.PortOfLoading).Distinct();
      var Carriers = bookingModels.Select(x => x.Carrier).Distinct();
      List<string> status = Enum.GetNames(typeof(OrderStatus)).ToList();
      SearchingManifestItem getSearchItemDTO = new SearchingManifestItem()
      {
        DestinationPort = DestinationPort,
        OriginPorts = OriginPorts,
        Carriers = Carriers,
        Status = status
      };
      return getSearchItemDTO;
    }
    public async Task<ShipmentManifestsDtos> CreateOrUpdateContainerAsync(ShipmentManifestsDtos rq)
    {
      Container container = new Container();
      container.Name = rq.Name;
      container.Size = rq.Size;
      container.Loading = rq.Loading;
      container.PackType = rq.PackType;
      container.Status = ContainerStatus.Pending;
      List<Manifest> manifests = new List<Manifest>();
      foreach (var item in rq.Manifests)
      {
        Manifest entity = new Manifest();
        if (item.selectedItem == true)
        {
          var orderDeatail = await _orderDetailRepository.Query(p => p.ItemNumber == item.ItemNumber, false).SelectAsync();
          var booking = await _shipmentBookingRepository.Query(p => p.Id == item.BookingId, false).SelectAsync();
          entity.Quantity = item.ShipQuantity;
          entity.Container = container;
          entity.Loading = container.Loading;
          entity.PackType = container.PackType;
          entity.Size = container.Size;
          entity.BookingId = item.BookingId;
          entity.Cartons = orderDeatail[0].Cartons;
          entity.Cube = orderDeatail[0].Cube;
          entity.FreightTerms = item.FreightTerms;
          entity.KGS = orderDeatail[0].KGS;
          if (item.ShipQuantity == item.OpenQuantity)
          {
            booking[0].Status = OrderStatus.Manifested;
            orderDeatail[0].Status = OrderStatus.Manifested;
          }
          if (item.ShipQuantity > 0 && item.ShipQuantity < item.OpenQuantity)
          {
            booking[0].Status = OrderStatus.PartlyManifested;
            orderDeatail[0].Status = OrderStatus.PartlyManifested;
          }
          entity.Booking = booking[0];

          manifests.Add(entity);
          _orderDetailRepository.Update(orderDeatail[0]);
          _shipmentBookingRepository.Update(booking[0]);
        }
      }
      if (manifests.Count > 0)
      {
        container.Manifests = manifests;
        _containerRepository.Insert(container);
      }
      await UnitOfWork.SaveChangesAsync();
      var rs = Mapper.Map<ShipmentManifestsDtos>(container);
      return rs;
    }


  }
}
