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

    public ManifestService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Manifest> manifestDataProvider, IManifestRepository manifestRepository, IDataProvider<Booking> bookingDataProvider,IShipmentBookingRepository shipmentBookingRepository) : base(unitOfWork, mapper, appContext)
    {
      this._manifestDataProvider = manifestDataProvider;
      this._manifestRepository = manifestRepository;

      this._shipmentBookingDataProvider = bookingDataProvider;
      this._shipmentBookingRepository = shipmentBookingRepository;
    }

    public async Task<PagedListResult<ShipmentManifestsDtos>> ListManifestDtoAsync(string DestinationPort=null,string OriginPort=null, string Carrier=null, string ETDFrom=null, string ETDTo=null, string Status=null,string Vendor=null, string PONumber=null,string Item =null )
    {
      PagedListResult<ShipmentManifestsDtos> pageResult = new PagedListResult<ShipmentManifestsDtos>();
      List<ShipmentManifestsDtos> shipmentManifestsDtos = new List<ShipmentManifestsDtos>();
      //value return
      var bookings = await _shipmentBookingDataProvider.ListAsync();
      var manifest = await _manifestDataProvider.ListAsync(null,null,true,null,null);
      List<Booking> listBooking = bookings.Items;
      List<Manifest> listManifest = manifest.Items;
      List<string> originPorts = listBooking.Select(p => p.PortOfLoading).ToList();
      List<DateTime> ETDs = listBooking.Select(p => p.ETD).ToList();
      List<string> destinationPorts = listBooking.Select(p => p.PortOfDelivery).ToList();
      List<string> containers = listManifest.Select(p => p.Container).ToList();
      if (containers.Count > 0)
      {
        foreach(var container in containers)
        {
          List<Manifest> manifests = listManifest.Where(p => p.Container == container).ToList();
          List<ItemManifest> itemManifests = new List<ItemManifest>();
          ShipmentManifestsDtos shipmentManifestsDto = new ShipmentManifestsDtos();
          foreach (var item in manifests)
          {
            ItemManifest itemManifest = new ItemManifest()
            {
              Supplier=item.Booking.Factory,
              Carrier=item.Booking.Carrier,
              PONumber=item.Booking.PONumber,
              ItemNumber=item.Booking.ItemNumber,
              ETDDate=item.Booking.ETD,
              BookingQuantity=item.Booking.Quantity,
              OpenQuantity=item.Booking.Quantity-item.Quantity,
              ShipQuantity=item.Quantity,
              BookingCartons=item.Booking.Quantity*(decimal)item.Booking.Cartons,
              ShipCartons=item.Quantity*(decimal)item.Cartons,
              BookingCube=item.Booking.Quantity*(decimal)item.Booking.Cube,
              ShipCube=item.Quantity*(decimal)item.Cube,
              NetWeight=item.Quantity*(decimal)item.KGS,
              Manifested=item.Booking.Status.ToString()
            };
            itemManifests.Add(itemManifest);
          }
          shipmentManifestsDto.itemManifests = itemManifests;
          shipmentManifestsDto.Container = container;
          
        }
      }     
      return pageResult;
    }
  }
}
