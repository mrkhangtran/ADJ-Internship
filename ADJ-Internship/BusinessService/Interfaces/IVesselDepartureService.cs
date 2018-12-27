using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using System;
using System.Threading.Tasks;
using static ADJ.BusinessService.Dtos.VesselDepartureDtos;

namespace ADJ.BusinessService.Interfaces
{
	public interface IVesselDepartureService
	{
		Task<PagedListResult<ContainerDto>> pagedListContainerAsync(OrderStatus Status, DateTime ETDFroms, DateTime ETDTos,
			int pageIndex = 1, int pageSize = 2, string PONumberSearch = null,
			string ItemSearch = null, string Origins = null, string OriginPorts = null);

		Task<GetItemSearchDto> SearchItem();

		//Task<Container> CreateOrUpdateProgressCheckAsync(ContainerDto rq);.
		//Task<Booking> FillterVesselDepartureAsync(BookingDto rq);
	}
}






