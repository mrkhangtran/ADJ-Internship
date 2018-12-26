using ADJ.BusinessService.Dtos;
using ADJ.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
  public interface IManifestService
  {
    Task<PagedListResult<ShipmentManifestsDtos>> ListManifestDtoAsync(int pageIndex = 1, int pageSize = 2, string DestinationPort = null, string OriginPort = null, string Carrier = null, DateTime? ETDFrom = null, DateTime? ETDTo = null, string Status = null, string Vendor = null, string PONumber = null, string Item = null);
    Task<SearchingManifestItem> SearchItem();
    Task<ShipmentManifestsDtos> CreateOrUpdateContainerAsync(ShipmentManifestsDtos rq);
  }
}
  
