using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.DeliveryTrack;
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
    public async Task<PagedListResult<DCBookingDtos>> ListDCBookingDtosAsync(int pageIndex = 1, int pageSize = 10, string DestinationPort = null, string bookingref = null, DateTime? bookingdatefrom = null, DateTime? bookingdateto = null, string DC = null, DateTime? arrivaldatefrom = null, DateTime? arrivaldateto = null, string Status = null, string Container = null)
    {
      PagedListResult<DCBookingDtos> pagedListResult = new PagedListResult<DCBookingDtos>();
      Expression<Func<Container, bool>> All = c => c.Id > 0;
      if (DestinationPort != null)
      {
        Expression<Func<Container, bool>> filter = x => x.ArriveOfDespatch.DestinationPort == DestinationPort;
        All = All.And(filter);
      }
      if (DC != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCBooking.DistributionCenter == DC;
        All = All.And(filter);
      }
      if (bookingref != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCBooking.BookingRef == bookingref;
        All = All.And(filter);
      }
      if (bookingdatefrom != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCBooking.BookingDate.CompareTo(bookingdatefrom) > 0;
        All = All.And(filter);
      }
      if (bookingdateto != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCBooking.BookingDate.CompareTo(bookingdateto) < 0;
        All = All.And(filter);
      }
      if (arrivaldatefrom != null)
      {
        Expression<Func<Container, bool>> filter = x => x.CA.ArrivalDate.CompareTo(arrivaldatefrom) > 0;
        All = All.And(filter);
      }
      if (arrivaldateto != null)
      {
        Expression<Func<Container, bool>> filter = x => x.CA.ArrivalDate.CompareTo(arrivaldateto) < 0;
        All = All.And(filter);
      }
      if (Status != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Status.ToString() == Status;
        All = All.And(filter);
      }
      if (Container != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Name == Container;
        All = All.And(filter);
      }
      var listContainer = await _containerDataProvider.ListAsync(All, null, true, pageIndex, pageSize);
      List<Container> containers = listContainer.Items;
      List<DCBookingDtos> dCBookingDtos = new List<DCBookingDtos>();
      foreach (var container in containers)
      {
        var confirmArrival = await _confirmArrivalRepository.Query(x => x.ContainerId == container.Id, false).SelectAsync();
        var arriveOfDispatch = await _arriveOfDespatchRepository.Query(x => x.ContainerId == container.Id, false).SelectAsync();
        var dcBooking = await _dcBookingRepository.Query(x => x.ContainerId == container.Id, false).SelectAsync();
        if (confirmArrival.Count() > 0 && arriveOfDispatch.Count() > 0)
        {
          DCBookingDtos dCBookingDto = new DCBookingDtos()
          {
            ContainerId = container.Id,
            Name = container.Name,
            DestPort = arriveOfDispatch[0].DestinationPort,
            ArrivalDate = confirmArrival[0].ArrivalDate,
            Status = container.Status.ToString(),
            BookingDate = DateTime.Now.Date
          };
          foreach (var manifest in container.Manifests)
          {
            dCBookingDto.ShipCarton += manifest.Quantity * (decimal)manifest.Cartons;
            dCBookingDto.ShipCube += manifest.Quantity * (decimal)manifest.Cube;
            dCBookingDto.ShipQuantity += manifest.Quantity;
          }
          if (dcBooking.Count > 0)
          {
            dCBookingDto.Id = dcBooking[0].Id;
            dCBookingDto.DistributionCenter = dcBooking[0].DistributionCenter;
            dCBookingDto.Haulier = dcBooking[0].Haulier;
            dCBookingDto.Client = dcBooking[0].Client;
            dCBookingDto.BookingDate = dcBooking[0].BookingDate;
            dCBookingDto.BookingRef = dcBooking[0].BookingRef;
            dCBookingDto.BookingTime = dcBooking[0].BookingTime;
          }
          dCBookingDtos.Add(dCBookingDto);
        }
      }
      pagedListResult.Items = dCBookingDtos;
      pagedListResult.TotalCount = listContainer.TotalCount;
      pagedListResult.PageCount = listContainer.PageCount;
      return pagedListResult;
    }
    public async Task<SearchingDCBooking> getItem()
    {
      var list =  _containerRepository.Query(true).SelectAsync(x => x.ArriveOfDespatch.DestinationPort).Result.Distinct();
      List<string> status = Enum.GetNames(typeof(ContainerStatus)).ToList();
      SearchingDCBooking searchingDCBooking = new SearchingDCBooking()
      {
        DestinationPort = list,
        Status = status
      };
      return searchingDCBooking;
    }
    public async Task<DCBookingDtos> CreateOrUpdate(DCBookingDtos rq)
    {
      DCBooking entity;
      entity = Mapper.Map<DCBooking>(rq);
      _dcBookingRepository.Insert(entity);
      var containers = await _containerRepository.Query(x => x.Id == rq.ContainerId, true).SelectAsync();
      var CA = await _confirmArrivalRepository.Query(x => x.ContainerId == rq.ContainerId, false).SelectAsync();
      var Arrive = await _arriveOfDespatchRepository.Query(x => x.ContainerId == rq.ContainerId, false).SelectAsync();

      Container container = containers[0];
      container.Status = ContainerStatus.DCBookingReceived;
      _containerRepository.Update(container);
      await UnitOfWork.SaveChangesAsync();
      var rs = Mapper.Map<DCBookingDtos>(entity);
      rs.Name = container.Name;
      rs.DestPort = Arrive[0].DestinationPort;
      rs.ArrivalDate = CA[0].ArrivalDate;
      foreach (var manifest in container.Manifests)
      {
        rs.ShipCarton += manifest.Quantity * (decimal)manifest.Cartons;
        rs.ShipCube += manifest.Quantity * (decimal)manifest.Cube;
        rs.ShipQuantity += manifest.Quantity;
      }
      rs.Status = container.Status.ToString();
      return rs;
    }
  }
}
