using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.OrderTrack;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
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
  public class ConfirmArrivalService : ServiceBase, IConfirmArrivalService
  {
    private readonly IDataProvider<Container> _containerDataProvider;
    private readonly int pageSize;

    public ConfirmArrivalService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
      IDataProvider<Container> containerDataProvider) : base(unitOfWork, mapper, appContext)
    {
      _containerDataProvider = containerDataProvider;

      pageSize = 6;
    }

    public async Task<List<ConfirmArrivalResultDtos>> ListContainerFilterAsync(int? page, DateTime? ETAFrom, DateTime? ETATo, string origin = null, string mode = null,
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
        Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.Mode == mode).Count() > 0;
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
          Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.ETA.CompareTo(ETAFrom) > 0).Count() > 0;
          All = All.And(filter);
        }

        if (ETATo != null)
        {
          Expression<Func<Container, bool>> filter = x => x.Manifests.Where(p => p.Booking.ETA.CompareTo(ETATo) < 0).Count() > 0;
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

      //NOT YET adding Orderby

      PagedListResult<Container> result = await _containerDataProvider.ListAsync(All, null, true, page, pageSize);

      return ConvertToResultAsync(result.Items);
    }

    public List<ConfirmArrivalResultDtos> ConvertToResultAsync(List<Container> containers)
    {
      List<ConfirmArrivalResultDtos> result = new List<ConfirmArrivalResultDtos>();

      foreach (var item in containers)
      {
        ConfirmArrivalResultDtos output = new ConfirmArrivalResultDtos();

        output.DestinationPort = (item.Manifests.ToList())[0].Booking.PortOfDelivery;
        output.Origin = (item.Manifests.ToList())[0].Booking.Order.Origin;
        output.Mode = (item.Manifests.ToList())[0].Booking.Mode;
        output.Carrier = (item.Manifests.ToList())[0].Booking.Carrier;
        output.ArrivalDate = (item.Manifests.ToList())[0].Booking.ETA;

        output.Vendor = (item.Manifests.ToList())[0].Booking.Order.Vendor;
        output.Container = item.Name;
        output.Status = item.Status;

        var test = typeof(ConfirmArrivalResultDtos).GetProperties()[0].GetValue(output);


        result.Add(output);
      }

      return Sort(result);
    }

    private List<ConfirmArrivalResultDtos> Sort(List<ConfirmArrivalResultDtos> input)
    {
      //Order by Destination Port (property number = 0)
      input = Quick_Sort(input, 0, input.Count, 0);

      //Order by Origin (property number = 1)
      //Order by Mode (property number = 2)
      //Order by Carrier (property number = 3)
      //Order by Arrival Date (property number = 4)
      for (int property = 1; property < 5; property ++)
      {
        int start = 0;
        for (int i = 0; i < input.Count; i++)
        {
          if (i + 1 < input.Count)
          {
            var currentProperty = typeof(ConfirmArrivalResultDtos).GetProperties()[property];
            if (currentProperty.GetValue(input[i]).ToString().CompareTo(currentProperty.GetValue(input[i + 1]).ToString()) != 0)
            {
              input = Quick_Sort(input, start, i + 1, property);
              start = i + 1;
            }
          }
        }
        input = Quick_Sort(input, start, input.Count, property);
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
      ConfirmArrivalResultDtos pivot = arr[left];
      while (true)
      {
        var currentProperty = typeof(ConfirmArrivalResultDtos).GetProperties()[property];
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
          if (arr[left] == arr[right]) return right;

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
  }
}
