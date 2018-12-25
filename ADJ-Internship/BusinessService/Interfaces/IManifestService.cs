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
    Task<PagedListResult<ShipmentManifestsDtos>> ListManifestDtoAsync(string DestinationPort = null, string OriginPort = null, string Carrier = null, string ETDFrom = null, string ETDTo = null, string Status = null, string Vendor = null, string PONumber = null, string Item = null);
  }
}
