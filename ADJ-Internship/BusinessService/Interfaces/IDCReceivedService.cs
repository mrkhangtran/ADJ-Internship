using ADJ.BusinessService.Dtos;
using ADJ.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
  public interface IDCReceivedService
  {
    Task<PagedListResult<DCReceivedResultDtos>> ListContainerFilterAsync(int? page, string container, string DC, DateTime? bookingDateFrom, DateTime? bookingDateTo,
      DateTime? deliveryDateFrom, DateTime? deliveryDateTo, string bookingRef, string status);
    Task<DCReceivedResultDtos> CreateOrUpdateCAAsync(DCReceivedResultDtos input);
  }
}
