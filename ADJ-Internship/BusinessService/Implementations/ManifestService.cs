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
      List<Booking> listBooking = bookings.Items;
      List<Booking> listBookingFilter = new List<Booking>();
      List<string> originPorts = new List<string>()
      {
        "Hong Kong"
      };
      List<DateTime> ETDs = listBooking.Select(p => p.ETD).ToList();
      List<string> destinationPorts = new List<string>()
      {
        "Hong Kong"
      };
      //foreach(var item in container){}
      //if(List<Booking>.Where(manifest==null)){
      foreach(var originPort in originPorts)
      {
        foreach(var ETD in ETDs)
        {
          foreach (var destinationPort in destinationPorts) 
          {
            ShipmentManifestsDtos shipmentManifestDto = new ShipmentManifestsDtos();
            ItemManifest itemManifest = new ItemManifest();
            listBookingFilter = listBooking.Where(p => p.ETD == ETD).ToList();
            if (listBookingFilter != null)
            {
              foreach(var booking in listBooking)
              {

              }
            }
          }
        }
      }
      //}
      return null;
    }
  }
}
