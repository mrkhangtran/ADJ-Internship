using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
	public interface IVesselDepartureService
	{
		Task<PagedListResult<ContainerDto>> ListManifestDtoAsync(string origin, string originPort, string container, string status, DateTime etdFrom, DateTime etdTo, int? pageIndex, int? pageSize);
		Task<SearchItem> SearchItem();
		//Task<ShipmentManifestsDtos> CreateOrUpdateContainerAsync(ShipmentManifestsDtos rq);

	}
}






