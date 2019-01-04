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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
  public class DCReceivedService : ServiceBase, IDCReceivedService
  {
    private readonly IDataProvider<Container> _containerDataProvider;

    private readonly IDCBookingRepository _dcBookingRepository;
    private readonly IDCReceivedRepository _dcRecievedRepository;
    private readonly IContainerRepository _containerRepository;

    private readonly int pageSize;

    public DCReceivedService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
      IDataProvider<Container> containerDataProvider, 
      IDCBookingRepository dcBookingRepository, IDCReceivedRepository dcRecievedRepository, IContainerRepository containerRepository) : base(unitOfWork, mapper, appContext)
    {
      _containerDataProvider = containerDataProvider;

      _dcBookingRepository = dcBookingRepository;
      _dcRecievedRepository = dcRecievedRepository;
      _containerRepository = containerRepository;

      pageSize = 12;
    }

    public async Task<PagedListResult<DCReceivedResultDtos>> ListContainerFilterAsync(int? page, string container, string DC, DateTime? bookingDateFrom, DateTime? bookingDateTo, 
      DateTime? deliveryDateFrom, DateTime? deliveryDateTo, string bookingRef, string status)
    {
      if (page == null) { page = 1; }

      Expression<Func<Container, bool>> All = x => x.Id > 0;

      if (container != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Name == container;
        All = All.And(filter);
      }

      if (DC != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCBooking.DistributionCenter == DC;
        All = All.And(filter);
      }

      if (bookingDateFrom != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCBooking.BookingDate >= bookingDateFrom;
        All = All.And(filter);
      }

      if (bookingDateTo != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCBooking.BookingDate <= bookingDateTo;
        All = All.And(filter);
      }

      if (deliveryDateFrom != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCConfirmation.DeliveryDate >= deliveryDateFrom;
        All = All.And(filter);
      }

      if (deliveryDateTo != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCConfirmation.DeliveryDate <= deliveryDateTo;
        All = All.And(filter);
      }

      if (bookingRef != null)
      {
        Expression<Func<Container, bool>> filter = x => x.DCBooking.BookingRef == bookingRef;
        All = All.And(filter);
      }

      if (status != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Status.ToString() == status;
        All = All.And(filter);
      }
      else
      {
        Expression<Func<Container, bool>> All1 = x => x.Status == ContainerStatus.DCBookingReceived;
        All1 = All.And(All1);
        Expression<Func<Container, bool>> All2 = x => x.Status == ContainerStatus.Delivered;
        All2 = All.And(All2);

        All = All1.Or(All2);
      }

      PagedListResult<Container> result = await _containerDataProvider.ListAsync(All, null, true, page, pageSize);

      PagedListResult<DCReceivedResultDtos> rs = new PagedListResult<DCReceivedResultDtos>();
      rs.Items = await ConvertToResultAsync(result.Items);
      rs.PageCount = result.PageCount;
      rs.TotalCount = result.TotalCount;

      return rs;
    }

    private async Task<List<DCReceivedResultDtos>> ConvertToResultAsync(List<Container> containers)
    {
      List<DCReceivedResultDtos> result = new List<DCReceivedResultDtos>();

      foreach (var item in containers)
      {
        DCReceivedResultDtos output = new DCReceivedResultDtos();
        List<DCBooking> booking = await _dcBookingRepository.Query(x => x.ContainerId == item.Id, false).SelectAsync();
        List<DCConfirmation> confirmation = await _dcRecievedRepository.Query(x => x.ContainerId == item.Id, false).SelectAsync();

        output.ContainerId = item.Id;
        output.Container = item.Name;

        output.DC = booking[0].DistributionCenter;
        output.Haulier = booking[0].Haulier;
        output.BookingDate = booking[0].BookingDate;
        output.BookingTime = booking[0].BookingTime;
        output.BookingRef = booking[0].BookingRef;

        if (item.Status == ContainerStatus.Arrived)
        {
          output.DeliverDate = confirmation[0].DeliveryDate;
          output.DeliverTime = confirmation[0].DeliveryTime;
        }

        output.Status = item.Status;

        result.Add(output);
      }

      return result;
    }

    public async Task<DCReceivedDtos> CreateOrUpdateCAAsync(DCReceivedResultDtos input)
    {
      DCConfirmation entity = await GetDCConfirmationbyContainerId(input.ContainerId);

      if (entity != null)
      {
        entity.DeliveryDate = input.DeliverDate;
        entity.DeliveryTime = input.DeliverTime;

        _dcRecievedRepository.Update(entity);
      }
      else
      {
        entity = Mapper.Map<DCConfirmation>(input);

        _dcRecievedRepository.Insert(entity);
      }

      await UpdateContainer(input.ContainerId);

      await UnitOfWork.SaveChangesAsync();

      return Mapper.Map<DCReceivedDtos>(entity);
    }

    public async Task<DCConfirmation> GetDCConfirmationbyContainerId(int containerId)
    {
      List<DCConfirmation> findCA = await _dcRecievedRepository.Query(x => x.ContainerId == containerId, false).SelectAsync();

      if (findCA.Count != 0)
      {
        return findCA[0];
      }
      else
      {
        return null;
      }
    }

    private async Task<Container> UpdateContainer(int containerId)
    {
      Container container = await _containerDataProvider.GetByIdAsync(containerId);

      container.Status = ContainerStatus.Delivered;
      _containerRepository.Update(container);

      return container;
    }
  }
}
