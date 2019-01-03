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

    private readonly IConfirmArrivalRepository _confirmArrivalRepository;

    private readonly int pageSize;

    public ConfirmArrivalService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
      IDataProvider<Container> containerDataProvider, IDataProvider<Booking> bookingDataProvider, IDataProvider<Order> orderDataProvider,
    IConfirmArrivalRepository confirmArrivalRepository) : base(unitOfWork, mapper, appContext)
    {
      _containerDataProvider = containerDataProvider;
      _bookingDataProvider = bookingDataProvider;
      _orderDataProvider = orderDataProvider;

      _confirmArrivalRepository = confirmArrivalRepository;

      pageSize = 6;
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
        Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.Order.Vendor == vendor).Count() > 0;
        All = All.And(filter);
      }

      if (container != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Name == container;
        All = All.And(filter);
      }

      if ((ETAFrom != null) || (ETATo != null))
      {
        status = ContainerStatus.Despatch.ToString();

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
        Expression<Func<Container, bool>> filter = x => x.Status.ToString() == status;
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

        output.DestinationPort = booking.PortOfDelivery;
        output.Origin = order.Origin;
        output.Mode = item.Loading;
        output.Carrier = booking.Carrier;
        output.ArrivalDate = booking.ETA;

        output.Vendor = order.Vendor;
        output.Container = item.Name;
        output.Status = item.Status;

        output.Id = item.Id;

        var test = typeof(ConfirmArrivalResultDtos).GetProperties()[0].GetValue(output);


        result.Add(output);
      }

      return Sort(result);
    }

    private List<ConfirmArrivalResultDtos> Sort(List<ConfirmArrivalResultDtos> input)
    {
      //Order by Destination Port (property number = 0)
      input = Quick_Sort(input, 0, input.Count - 1, 0);

      //Order by Origin (property number = 1)
      //Order by Mode (property number = 2)
      //Order by Carrier (property number = 3)
      //Order by Arrival Date (property number = 4)
      for (int property = 1; property < 5; property++)
      {
        int start = 0;
        for (int i = 0; i < input.Count; i++)
        {
          if ((i + 1 < input.Count) || (i == input.Count - 1))
          {
            var currentProperty = typeof(ConfirmArrivalResultDtos).GetProperties()[property];
            if ((i == input.Count - 1) || (currentProperty.GetValue(input[i]).ToString().CompareTo(currentProperty.GetValue(input[i + 1]).ToString()) != 0))
            {
              input = Quick_Sort(input, start, i, property);
              start = i + 1;
            }
          }
        }
      }

      return input;
    }

    private static List<ConfirmArrivalResultDtos> Quick_Sort(List<ConfirmArrivalResultDtos> arr, int left, int right, int property)
    {
      if (left < right)
      {
        int pivot = Partition(arr, left, right, property);

        if (pivot > 1)
        {
          Quick_Sort(arr, left, pivot - 1, property);
        }
        if (pivot + 1 < right)
        {
          Quick_Sort(arr, pivot + 1, right, property);
        }
      }

      return arr;
    }

    private static int Partition(List<ConfirmArrivalResultDtos> arr, int left, int right, int property)
    {
      var currentProperty = typeof(ConfirmArrivalResultDtos).GetProperties()[property];

      string pivot = currentProperty.GetValue(arr[left]).ToString();

      while (true)
      {
        while (currentProperty.GetValue(arr[left]).ToString().CompareTo(pivot) < 0)
        {
          left++;
        }

        while (currentProperty.GetValue(arr[right]).ToString().CompareTo(pivot) > 0)
        {
          right--;
        }

        if (left < right)
        {
          if (currentProperty.GetValue(arr[left]).ToString().Equals(currentProperty.GetValue(arr[right]).ToString())) return right;

          ConfirmArrivalResultDtos temp = arr[left];
          arr[left] = arr[right];
          arr[right] = temp;
        }
        else
        {
          return right;
        }
      }
    }

    public async Task<ConfirmArrivalDtos> CreateOrUpdateCAAsync(int containerId, DateTime arrivalDate)
    {
      CA entity;
      ConfirmArrivalDtos input = new ConfirmArrivalDtos();

      input.ContainerId = containerId;
      input.ArrivalDate = arrivalDate;

      CA ca = await GetCAbyContainerId(containerId);

      if (ca != null)
      {
        entity = await _confirmArrivalRepository.GetByIdAsync(containerId, false);
        if (entity == null)
        {
          throw new AppException("Purchase Order Not Found");
        }

        entity = Mapper.Map(input, entity);

        _confirmArrivalRepository.Update(entity);
      }
      else
      {
        entity = Mapper.Map<CA>(input);

        _confirmArrivalRepository.Insert(entity);
      }

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
  }
}
