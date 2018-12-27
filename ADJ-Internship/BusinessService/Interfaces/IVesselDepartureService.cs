using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
	public interface IVesselDepartureService
	{
		Task<PagedListResult<Container>> ListProgressCheckDtoAsync(int pageIndex = 1, int pageSize = 2, 
			string PONumberSearch = null, string ItemSearch = null, 
			string Origins = null, string OriginPorts = null, string Status,string ETDFroms,);

		Task<GetItemSearchDto> SearchItem();

		//Task<Container> CreateOrUpdateProgressCheckAsync(ContainerDto rq);.
		Task<Container> FillterVesselDepartureAsync(ContainerDto rq);
	}
}






