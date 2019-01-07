using ADJ.BusinessService.Dtos;
using ADJ.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
  public interface IDCBookingService
  {
    Task<PagedListResult<DCBookingDtos>> ListDCBookingDtosAsync(int pageIndex = 1, int pageSize = 10, string DestinationPort = null, string bookingref = null, DateTime? bookingdatefrom = null, DateTime? bookingdateto = null, string DC = null, DateTime? arrivaldatefrom = null, DateTime? arrivaldateto = null, string Status = null, string Container = null);
    Task<SearchingDCBooking> getItem();
    Task<DCBookingDtos> CreateOrUpdate(DCBookingDtos rq);
  }
}
