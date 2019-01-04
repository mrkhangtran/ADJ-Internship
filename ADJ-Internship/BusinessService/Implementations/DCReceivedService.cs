using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.DeliveryTrack;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
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

    private readonly int pageSize;

    public DCReceivedService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext) : base(unitOfWork, mapper, appContext)
    {
      pageSize = 12;
    }

    public async Task<PagedListResult<DCReceivedDtos>> ListContainerFilterAsync(int? page, string container, string DC, DateTime? bookingDateFrom, DateTime? bookingDateTo, 
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

      return null;
    }
  }
}
