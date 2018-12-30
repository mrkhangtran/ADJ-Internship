using ADJ.BusinessService.Core;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
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

    public ConfirmArrivalService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
      IDataProvider<Container> containerDataProvider) : base(unitOfWork, mapper, appContext)
    {
      _containerDataProvider = containerDataProvider;
    }

    public async Task<List<Container>> ListContainerFilterAsync(int? page, DateTime? ETAFrom, DateTime? ETATo, string origin = null, string mode = null, 
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

      //NOT YET adding Orderby

      PagedListResult<Container> result = await _containerDataProvider.ListAsync(All, "PortOfDelivery", true);

      return result.Items;
    }


  }
}
