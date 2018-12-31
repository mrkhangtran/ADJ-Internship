using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
	public interface IVesselDepartureService
	{
		Task<PagedListResult<ContainerDto>> ListContainerAsync(int pageIndex = 1, int pageSize = 2, string Origin = null, string OriginPort = null, string Loading = null, DateTime? ETDFrom = null, DateTime? ETDTo = null, string Status = null, string Vendor = null, string PONumber = null, string Item = null,string Warehouse=null);
		Task<SearchItem> SearchItem();
		Task<ShipmentManifestsDtos> CreateOrUpdateContainerAsync(ShipmentManifestsDtos rq);
	}
}






