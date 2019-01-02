using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
  public interface IConfirmArrivalService
  {
    Task<PagedListResult<ConfirmArrivalResultDtos>> ListContainerFilterAsync(int? page, DateTime? ETAFrom, DateTime? ETATo, string origin = null, string mode = null,
      string vendor = null, string container = null, string status = null);
    List<ConfirmArrivalResultDtos> ConvertToResultAsync(List<Container> containers);

    Task<ConfirmArrivalDtos> CreateOrUpdateCAAsync(int containerId, DateTime arrivalDate);
  }
}
