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
    Task<PagedListResult<DCBookingDtos>> ListDCBookingDtosAsync();
  }
}
