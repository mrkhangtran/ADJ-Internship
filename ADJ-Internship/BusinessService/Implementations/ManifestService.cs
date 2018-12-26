using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
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

    public ManifestService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Manifest> manifestDataProvider, IManifestRepository manifestRepository, IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository shipmentBookingRepository, IContainerRepository containerRepository, IDataProvider<Container> containerDataProvider) : base(unitOfWork, mapper, appContext)
    {
      this._manifestDataProvider = manifestDataProvider;
      this._manifestRepository = manifestRepository;

      this._shipmentBookingDataProvider = bookingDataProvider;
      this._shipmentBookingRepository = shipmentBookingRepository;

      this._containerRepository = containerRepository;
      this._containerDataProvider = containerDataProvider;

    }

    public async Task<PagedListResult<ShipmentManifestsDtos>> ListManifestDtoAsync(string DestinationPort = null, string OriginPort = null, string Carrier = null, string ETDFrom = null, string ETDTo = null, string Status = null, string Vendor = null, string PONumber = null, string Item = null)
    {
      PagedListResult<ShipmentManifestsDtos> pageResult = new PagedListResult<ShipmentManifestsDtos>();
      List<ShipmentManifestsDtos> shipmentManifestsDtos = new List<ShipmentManifestsDtos>();
      var containers = await _containerDataProvider.ListAsync(null, null, true);
      var bookings = await _shipmentBookingDataProvider.ListAsync();
      List<Booking> listBooking = bookings.Items;
      List<string> originPorts = listBooking.Select(p => p.PortOfLoading).ToList();
      List<DateTime> ETDs = listBooking.Select(p => p.ETD).ToList();
      List<string> destinationPorts = listBooking.Select(p => p.PortOfDelivery).ToList();

      //BookingHasManifestContainer
      foreach (var container in containers.Items)
      {
        List<ItemManifest> itemManifests = new List<ItemManifest>();
        ShipmentManifestsDtos shipmentManifestsDto = new ShipmentManifestsDtos();
        foreach (var item in container.Manifests)
        {
          ItemManifest itemManifest = new ItemManifest()
          {
            Id = item.Id,
            Supplier = item.Booking.Factory,
            Carrier = item.Booking.Carrier,
            PONumber = item.Booking.PONumber,
            ItemNumber = item.Booking.ItemNumber,
            ETDDate = item.Booking.ETD,
            BookingQuantity = item.Booking.Quantity,
            OpenQuantity = item.Booking.Quantity - item.Quantity,
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
        shipmentManifestsDto.itemManifests = itemManifests;
        shipmentManifestsDto.Container = container.Name;
        shipmentManifestsDto.Size = container.Size;
        shipmentManifestsDto.Loading = container.Loading;
        shipmentManifestsDto.PackType = container.PackType;
        shipmentManifestsDtos.Add(shipmentManifestsDto);
      }

      //BookingHasn'tManifestContainer
      List<Booking> listBookingNoContainer = listBooking.Where(p => p.Status == OrderStatus.BookingMade).OrderBy(p => p.PortOfLoading).ToList();
      ShipmentManifestsDtos shipmentNoContainerDto = new ShipmentManifestsDtos();
      List<ItemManifest> itemNoContainer = new List<ItemManifest>();
      foreach (var bookingNoContainer in listBookingNoContainer)
      {
        ItemManifest itemManifest = new ItemManifest()
        {
          Supplier = bookingNoContainer.Factory,
          Carrier = bookingNoContainer.Carrier,
          PONumber = bookingNoContainer.PONumber,
          ItemNumber = bookingNoContainer.ItemNumber,
          ETDDate = bookingNoContainer.ETD,
          BookingQuantity = bookingNoContainer.Quantity,
          BookingCartons = bookingNoContainer.Quantity * (decimal)bookingNoContainer.Cartons,
          BookingCube = bookingNoContainer.Quantity * (decimal)bookingNoContainer.Cube,
          Manifested = bookingNoContainer.Status.ToString(),
        };
        itemNoContainer.Add(itemManifest);
      }
      shipmentNoContainerDto.itemManifests = itemNoContainer;
      shipmentManifestsDtos.Add(shipmentNoContainerDto);
      pageResult.Items = shipmentManifestsDtos;
      pageResult.PageCount = (int)Math.Ceiling(decimal.Divide(shipmentManifestsDtos.Count, 2));
      return pageResult;
    }
  }
}
