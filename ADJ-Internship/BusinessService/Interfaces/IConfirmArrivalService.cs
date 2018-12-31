using ADJ.BusinessService.Dtos;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
  public interface IConfirmArrivalService
  {
    Task<List<ConfirmArrivalResultDtos>> ListContainerFilterAsync(int? page, DateTime? ETAFrom, DateTime? ETATo, string origin = null, string mode = null,
      string vendor = null, string container = null, string status = null);
    List<ConfirmArrivalResultDtos> ConvertToResultAsync(List<Container> containers);

    Task<ConfirmArrivalDtos> CreateOrUpdateBookingAsync(int containerId, DateTime arrivalDate);
  }
}
