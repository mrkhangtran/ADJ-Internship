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

    private readonly IArriveOfDespatchRepository _arriveOfDespatchRepository;
    private readonly IDataProvider<ArriveOfDespatch> _arriveOfDespatchDataProvider;

    private readonly int pageSize;


    public VesselDepartureService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
        IDataProvider<Manifest> manifestDataProvider, IManifestRepository manifestRepository,
        IDataProvider<ArriveOfDespatch> arriveOfDespatchDataProvider, IArriveOfDespatchRepository arriveOfDespatchRepository,
        IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository shipmentBookingRepository, IContainerRepository containerRepository,
        IDataProvider<Container> containerDataProvider) : base(unitOfWork, mapper, appContext)
    {
      this._manifestDataProvider = manifestDataProvider;
      this._manifestRepository = manifestRepository;

      this._shipmentBookingDataProvider = bookingDataProvider;
      this._shipmentBookingRepository = shipmentBookingRepository;

      this._containerRepository = containerRepository;
      this._containerDataProvider = containerDataProvider;

      this._arriveOfDespatchDataProvider = arriveOfDespatchDataProvider;
      this._arriveOfDespatchRepository = arriveOfDespatchRepository;

      pageSize = 50;
    }

    public async Task<PagedListResult<ContainerDto>> ListContainerDtoAsync(int? page, string origin, string originPort, string container, string status, DateTime? etdFrom, DateTime? etdTo)
    {
      if (page == null) { page = 1; }

      Expression<Func<Container, bool>> All = x => x.Id > 0;

      if (origin != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.Order.Origin == origin).Count() > 0;
        All = All.And(filter);
      }

      if (container != null)
      {
        Expression<Func<Container, bool>> filter = x => x.Name == container;
        All = All.And(filter);
      }

      Expression<Func<Container, bool>> All1 = x => x.Status == ContainerStatus.Pending;
      Expression<Func<Container, bool>> All2 = x => x.Status == ContainerStatus.Despatch;

      if (originPort != null)
      {
        Expression<Func<Container, bool>> filter1 = x => x.Manifests.Where(p => p.Booking.PortOfLoading == originPort).Count() > 0;
        All1 = All1.And(filter1);
        Expression<Func<Container, bool>> filter2 = x => x.ArriveOfDespatch.OriginPort == originPort;
        All2 = All2.And(filter2);
      }

      if (etdFrom != null)
      {
        Expression<Func<Container, bool>> filter1 = x => x.Manifests.Where(p => p.Booking.ETD >= etdFrom).Count() > 0;
        All1 = All1.And(filter1);
        Expression<Func<Container, bool>> filter2 = x => x.ArriveOfDespatch.ETD >= etdFrom;
        All2 = All2.And(filter2);
      }

      if (etdTo != null)
      {
        Expression<Func<Container, bool>> filter1 = x => x.Manifests.Where(p => p.Booking.ETD <= etdTo).Count() > 0;
        All1 = All1.And(filter1);
        Expression<Func<Container, bool>> filter2 = x => x.ArriveOfDespatch.ETD <= etdFrom;
        All2 = All2.And(filter2);
      }

      if (status == ContainerStatus.Pending.ToString())
      {
        All = All.And(All1);
      }
      else if (status == ContainerStatus.Despatch.ToString())
      {
        All = All.And(All2);
      }
      else
      {
        All = All.And(All1.Or(All2));
      }

      PagedListResult<Container> result = await _containerDataProvider.ListAsync(All, null, true, page, pageSize);

      PagedListResult<ContainerDto> rs = new PagedListResult<ContainerDto>();
      rs.Items = await ConvertToResultAsync(result.Items);
      rs.PageCount = result.PageCount;
      rs.TotalCount = result.TotalCount;

      return rs;
    }

    private async Task<List<ContainerDto>> ConvertToResultAsync(List<Container> input)
    {
      List<ContainerDto> result = new List<ContainerDto>();

      foreach (var item in input)
      {
        ContainerDto output = new ContainerDto();
        Booking booking = await _shipmentBookingDataProvider.GetByIdAsync((item.Manifests.ToList())[0].BookingId);
        List<ArriveOfDespatch> arriveOfDespatch = await _arriveOfDespatchRepository.Query(x => x.ContainerId == item.Id, false).SelectAsync();

        if (item.Status == ContainerStatus.Pending)
        {
          output = Mapper.Map<ContainerDto>(booking);
          output.OriginPort = booking.PortOfLoading;
          output.DestinationPort = booking.PortOfDelivery;
        }
        else if (item.Status == ContainerStatus.Despatch)
        {
          output = Mapper.Map<ContainerDto>(arriveOfDespatch[0]);
        }

        //output = Mapper.Map<ContainerDto>(item);
        output.Name = item.Name;
        output.Size = item.Size;
        output.Status = item.Status;

        output.ContainerId = item.Id;

        result.Add(output);
      }

      return Sort(result);
    }

    private List<ContainerDto> Sort(List<ContainerDto> input)
    {
      //Order by Origin Port (property number = 0)
      quickSort(input, 0, input.Count - 1, 0);

      //Order by Destination (property number = 1)
      //Order by Mode (property number = 2)
      //Order by Carrier (property number = 3)
      //Order by ETD (property number = 4)
      //Order by ETA (property number = 5)
      for (int property = 1; property < 6; property++)
      {
        int start = 0;
        for (int i = 0; i < input.Count; i++)
        {
          if ((i + 1 < input.Count) || (i == input.Count - 1))
          {
            bool different = false;
            if (i != input.Count - 1)
            {
              for (int j = property - 1; j >= 0; j--)
              {
                var currentProperty = typeof(ContainerDto).GetProperties()[j];
                if (currentProperty.GetValue(input[i]).ToString().CompareTo(currentProperty.GetValue(input[i + 1]).ToString()) != 0)
                {
                  different = true;
                  break;
                }
              }
            }

            if ((i == input.Count - 1) || (different))
            {
              quickSort(input, start, i, property);
              start = i + 1;
            }
          }
        }
      }

      return input;
    }

    public static void quickSort(List<ContainerDto> A, int left, int right, int property)
    {
      if (left > right || left < 0 || right < 0) return;

      int index = partition(A, left, right, property);

      if (index != -1)
      {
        quickSort(A, left, index - 1, property);
        quickSort(A, index + 1, right, property);
      }
    }

    private static int partition(List<ContainerDto> A, int left, int right, int property)
    {
      if (left > right) return -1;

      int end = left;

      switch (property)
      {
        case 4:
          {
            DateTime pivot = A[right].ETD;    // choose last one to pivot, easy to code
            for (int i = left; i < right; i++)
            {
              if (A[i].ETD.CompareTo(pivot) < 0)
              {
                swap(A, i, end);
                end++;
              }
            }
            break;
          }
        case 5:
          {
            DateTime pivot = A[right].ETA;    // choose last one to pivot, easy to code
            for (int i = left; i < right; i++)
            {
              if (A[i].ETA.CompareTo(pivot) < 0)
              {
                swap(A, i, end);
                end++;
              }
            }
            break;
          }
        default:
          {
            var currentProperty = typeof(ContainerDto).GetProperties()[property];
            string pivot = currentProperty.GetValue(A[right]).ToString();    // choose last one to pivot, easy to code
            for (int i = left; i < right; i++)
            {
              if (currentProperty.GetValue(A[i]).ToString().CompareTo(pivot) < 0)
              {
                swap(A, i, end);
                end++;
              }
            }
          }
          break;
      }

      swap(A, end, right);

      return end;
    }

    private static void swap(List<ContainerDto> A, int left, int right)
    {
      var tmp = A[left];
      A[left] = A[right];
      A[right] = tmp;
    }

    public async Task<ContainerDto> CreateOrUpdateAsync(ContainerDto input, ContainerInfoDto containerInfo)
    {
      ArriveOfDespatch entity = await GetArriveOfDespatchbyContainerId(input.ContainerId);

      if (entity != null)
      {
        entity.OriginPort = containerInfo.OriginPort;
        entity.DestinationPort = containerInfo.DestinationPort;
        entity.Mode = containerInfo.Mode;
        entity.Carrier = containerInfo.Carrier;

        _arriveOfDespatchRepository.Update(entity);
      }
      else
      {
        entity = Mapper.Map<ArriveOfDespatch>(input);

        entity.OriginPort = containerInfo.OriginPort;
        entity.DestinationPort = containerInfo.DestinationPort;
        entity.Mode = containerInfo.Mode;
        entity.Carrier = containerInfo.Carrier;

        _arriveOfDespatchRepository.Insert(entity);
      }

      await UpdateContainer(input.ContainerId);

      await UnitOfWork.SaveChangesAsync();

      return Mapper.Map<ContainerDto>(entity);
    }

    private async Task<ArriveOfDespatch> GetArriveOfDespatchbyContainerId(int containerId)
    {
      List<ArriveOfDespatch> findCA = await _arriveOfDespatchRepository.Query(x => x.ContainerId == containerId, false).SelectAsync();

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

      container.Status = ContainerStatus.Despatch;
      _containerRepository.Update(container);

      return container;
    }

  }
}
