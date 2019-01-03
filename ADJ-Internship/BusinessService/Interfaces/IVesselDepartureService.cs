using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
	public interface IVesselDepartureService
	{
		Task<VesselDepartureDtos> ListContainerDtoAsync(int? pageIndex, int? pageSize, string origin = null, string originPort = null, string container = null, string status = null, DateTime? etdFrom = null, DateTime? etdTo = null);
		Task<SearchItem> SearchItem();
		//Task<ShipmentManifestsDtos> CreateOrUpdateContainerAsync(ShipmentManifestsDtos rq);

	}
}






