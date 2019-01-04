using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.DeliveryTrack;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
  public class DCBookingService : ServiceBase, IDCBookingService
  {

    private readonly IContainerRepository _containerRepository;
    private readonly IDataProvider<Container> _containerDataProvider;

    private readonly IConfirmArrivalRepository _confirmArrivalRepository;
    private readonly IDataProvider<CA> _confirmArrivalDataProvider;

    private readonly IArriveOfDespatchRepository _arriveOfDespatchRepository;
    private readonly IDataProvider<ArriveOfDespatch> _arriveOfDespatchDataProvider;

    private readonly IDCBookingRepository _dcBookingRepository;
    private readonly IDataProvider<DCBooking> _dcBookingDataProvider;


    public DCBookingService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IContainerRepository containerRepository, IDataProvider<Container> containerDataProvider, IConfirmArrivalRepository confirmArrivalRepository, IDataProvider<CA> confirmArrivalDataProvider, IArriveOfDespatchRepository arriveOfDespatchRepository, IDataProvider<ArriveOfDespatch> arriveOfDespatchDataProvider, IDCBookingRepository dcBookingRepository, IDataProvider<DCBooking> dcBookingDataProvider) : base(unitOfWork, mapper, appContext)
    {
      this._containerRepository = containerRepository;
      this._containerDataProvider = containerDataProvider;

      this._confirmArrivalRepository = confirmArrivalRepository;
      this._confirmArrivalDataProvider = confirmArrivalDataProvider;

      this._arriveOfDespatchRepository = arriveOfDespatchRepository;
      this._arriveOfDespatchDataProvider = arriveOfDespatchDataProvider;

      this._dcBookingRepository = dcBookingRepository;
      this._dcBookingDataProvider = dcBookingDataProvider;
    }
    public async Task<PagedListResult<DCBookingDtos>> ListDCBookingDtosAsync()
    {
      PagedListResult<DCBookingDtos> pagedListResult = new PagedListResult<DCBookingDtos>();
      Expression<Func<Container, bool>> All = c => c.Id > 0;
      var listContainer = await _containerDataProvider.ListAsync(All, null, true);
      List<Container> containers = listContainer.Items;
      List<DCBookingDtos> dCBookingDtos = new List<DCBookingDtos>();
      foreach (var container in containers)
      {
        var confirmArrival = await _confirmArrivalRepository.Query(x => x.ContainerId == container.Id, true).SelectAsync();
        var arriveOfDispatch = await _arriveOfDespatchRepository.Query(x => x.ContainerId == container.Id, true).SelectAsync();
        var dcBooking = await _dcBookingRepository.Query(x => x.ContainerId == container.Id, true).SelectAsync();
        DCBookingDtos dCBookingDto = new DCBookingDtos()
        {
          Id = dcBooking[0].Id,
          ContainerId = container.Id,
          Name = container.Name,
          DestPort = arriveOfDispatch[0].DestinationPort,
          ArrivalDate = confirmArrival[0].ArrivalDate,
          Status = container.Status.ToString(),
        };
        foreach (var manifest in container.Manifests)
        {
          dCBookingDto.ShipCarton += manifest.Quantity * (decimal)manifest.Cartons;
          dCBookingDto.ShipCube += manifest.Quantity * (decimal)manifest.Cube;
          dCBookingDto.ShipQuantity += manifest.Quantity;
        }
        if (dcBooking[0] != null)
        {
          dCBookingDto.DistributionCenter = dcBooking[0].DistributionCenter;
          dCBookingDto.Haulier = dcBooking[0].Haulier;
          dCBookingDto.Client = dcBooking[0].Client;
          dCBookingDto.BookingDate = dcBooking[0].BookingDate;
          dCBookingDto.BookingRef = dcBooking[0].BookingRef;
        }
        dCBookingDtos.Add(dCBookingDto);
      }
      pagedListResult.Items = dCBookingDtos;
      return pagedListResult;
    }
  }
}
