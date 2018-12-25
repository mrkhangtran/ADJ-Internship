using ADJ.BusinessService.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
  public interface IShipmentBookingService
  {
    Task<List<OrderDetailDTO>> ListShipmentFilterAsync(int? page, string origin = null, string originPort = null, string mode = null, string warehouse = null,
      string status = null, string vendor = null, string poNumber = null, string itemNumber = null);
    Task<List<ShipmentResult>> ConvertToResult(List<OrderDetailDTO> input);
  }
}
