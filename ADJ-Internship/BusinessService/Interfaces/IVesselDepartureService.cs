using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
	public interface IVesselDepartureService
	{
		Task<VesselDepartureDtos> ListContainerDtoAsync(int? containerId, string origin = null, string originPort = null, string status = null, int? pageIndex = null, int? pageSize = null, DateTime? etdFrom = null, DateTime? etdTo = null);
		Task<SearchItem> SearchItem();
		VesselDepartureDtos Achive(VesselDepartureDtos model);

	}
}






