using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
  public interface IVesselDepartureService
  {
    Task<PagedListResult<ContainerDto>> ListContainerDtoAsync(int? page, string origin, string originPort, string container, string status, DateTime? etdFrom, DateTime? etdTo);
  }
}






