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
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
  public class ConfirmArrivalService : ServiceBase, IConfirmArrivalService
  {
    private readonly IDataProvider<Container> _containerDataProvider;
    private readonly IDataProvider<Booking> _bookingDataProvider;
    private readonly IDataProvider<Order> _orderDataProvider;

    private readonly IContainerRepository _containerRepository;
    private readonly IConfirmArrivalRepository _confirmArrivalRepository;
    private readonly IArriveOfDespatchRepository _arriveOfDespatchRepository;

    private readonly int pageSize;

    public ConfirmArrivalService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, 
      IDataProvider<Container> containerDataProvider, IDataProvider<Booking> bookingDataProvider, IDataProvider<Order> orderDataProvider,
    IContainerRepository containerRepository, IConfirmArrivalRepository confirmArrivalRepository, IArriveOfDespatchRepository arriveOfDespatchRepository) : base(unitOfWork, mapper, appContext)
    {
      _containerDataProvider = containerDataProvider;
      _bookingDataProvider = bookingDataProvider;
      _orderDataProvider = orderDataProvider;

      _containerRepository = containerRepository;
      _confirmArrivalRepository = confirmArrivalRepository;
      _arriveOfDespatchRepository = arriveOfDespatchRepository;

      pageSize = 50;
    }

    public async Task<PagedListResult<ConfirmArrivalResultDtos>> ListContainerFilterAsync(int? page, DateTime? ETAFrom, DateTime? ETATo, string origin = null, string mode = null,
      string vendor = null, string container = null, string status = null)
    {
      if (page == null) { page = 1; }

      Expression<Func<Container, bool>> All = x => x.Id > 0;

      if (origin != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.Order.Origin == origin).Count() > 0;
        All = All.And(filter);
      }

      if (mode != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Loading == mode;
        All = All.And(filter);
      }

      if (vendor != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.Order.Vendor.Contains(vendor)).Count() > 0;
        All = All.And(filter);
      }

      if (container != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Name.Contains(container);
        All = All.And(filter);
      }

      if ((ETAFrom != null) || (ETATo != null))
      {
        status = ContainerStatus.Despatch.GetDescription<ContainerStatus>();

        if (ETAFrom != null)
        {
          Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.ETA >= ETAFrom).Count() > 0;
          All = All.And(filter);
        }

        if (ETATo != null)
        {
          Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.ETA <= ETATo).Count() > 0;
          All = All.And(filter);
        }
      }

      if (status != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Status.GetDescription<ContainerStatus>() == status;
        All = All.And(filter);
      }
      else
      {
        Expression<Func<Container, bool>> All1 = x => x.Status == ContainerStatus.Despatch;
        All1 = All.And(All1);
        Expression<Func<Container, bool>> All2 = x => x.Status == ContainerStatus.Arrived;
        All2 = All.And(All2);

        All = All1.Or(All2);
      }

      Expression<Func<Container, bool>> test = x => x.Id > 0;

      PagedListResult<Container> result = await _containerDataProvider.ListAsync(All, null, true, page, pageSize);

      PagedListResult<ConfirmArrivalResultDtos> rs = new PagedListResult<ConfirmArrivalResultDtos>();
      rs.Items = await ConvertToResultAsync(result.Items);
      rs.PageCount = result.PageCount;
      rs.TotalCount = result.TotalCount;

      return rs;
    }

    public async Task<List<ConfirmArrivalResultDtos>> ConvertToResultAsync(List<Container> containers)
    {
      List<ConfirmArrivalResultDtos> result = new List<ConfirmArrivalResultDtos>();

      foreach (var item in containers)
      {
        ConfirmArrivalResultDtos output = new ConfirmArrivalResultDtos();
        Booking booking = await _bookingDataProvider.GetByIdAsync((item.Manifests.ToList())[0].BookingId);
        Order order = await _orderDataProvider.GetByIdAsync(booking.OrderId);
        List<CA> confirmArrival = await _confirmArrivalRepository.Query(x => x.ContainerId == item.Id, false).SelectAsync();
        List<ArriveOfDespatch> arriveOfDespatch = await _arriveOfDespatchRepository.Query(x => x.ContainerId == item.Id, false).SelectAsync();

        //output.DestinationPort = booking.PortOfDelivery;
        output.DestinationPort = arriveOfDespatch[0].DestinationPort;
        output.Origin = order.Origin;
        output.Mode = item.Loading;
        //output.Carrier = booking.Carrier;
        output.Carrier = arriveOfDespatch[0].Carrier;
        output.ETD = booking.ETD;

        output.Vendor = order.Vendor;
        output.Container = item.Name;
        output.Status = item.Status;

        if (output.Status == ContainerStatus.Despatch)
        {
          output.ArrivalDate = booking.ETA;
        }
        else
        {
          output.ArrivalDate = confirmArrival[0].ArrivalDate;
        }

        output.Id = item.Id;

        var test = typeof(ConfirmArrivalResultDtos).GetProperties()[0].GetValue(output);


        result.Add(output);
      }

      result = result.OrderBy(p => p.DestinationPort).ThenBy(p => p.Origin).ThenBy(p => p.Mode).ThenBy(p => p.Carrier).ThenBy(p => p.ArrivalDate).ThenBy(p => p.Container).ToList();

      return result;

    }

    public async Task<ConfirmArrivalDtos> CreateOrUpdateCAAsync(int containerId, DateTime arrivalDate)
    {
      CA entity = await GetCAbyContainerId(containerId);
      ConfirmArrivalDtos input = new ConfirmArrivalDtos();

      input.ContainerId = containerId;
      input.ArrivalDate = arrivalDate;

      if (entity != null)
      {
        entity.ArrivalDate = input.ArrivalDate;

        _confirmArrivalRepository.Update(entity);
      }
      else
      {
        entity = Mapper.Map<CA>(input);

        _confirmArrivalRepository.Insert(entity);
      }

      await UpdateContainer(containerId, arrivalDate);

      await UnitOfWork.SaveChangesAsync();

      return Mapper.Map<ConfirmArrivalDtos>(entity);
    }

    public async Task<CA> GetCAbyContainerId(int containerId)
    {
      List<CA> findCA = await _confirmArrivalRepository.Query(x => x.ContainerId == containerId, false).SelectAsync();

      if (findCA.Count != 0)
      {
        return findCA[0];
      }
      else
      {
        return null;
      }
    }

    private async Task<Container> UpdateContainer(int containerId, DateTime arrivalDate)
    {
      Container container = await _containerDataProvider.GetByIdAsync(containerId);

      container.Status = ContainerStatus.Arrived;
      _containerRepository.Update(container);

      return container;
    }
  }
}
