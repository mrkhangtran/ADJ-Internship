using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.OrderTrack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
  public interface IProgressCheckService
  {
    Task<PagedListResult<ProgressCheckDto>> ListProgressCheckDtoAsync(int pageIndex = 1, int pageSize = 2, string PONumberSearch = null, string ItemSearch = null,
      string Suppliers = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null);
    Task<GetItemSearchDto> SearchItem();
    Task<ProgressCheckDto> CreateOrUpdateProgressCheckAsync(ProgressCheckDto rq);
  }
}
