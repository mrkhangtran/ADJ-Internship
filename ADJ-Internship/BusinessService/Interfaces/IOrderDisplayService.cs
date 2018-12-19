using ADJ.BusinessService.Dtos;
using ADJ.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
	public interface IOrderDisplayService
	{
		Task<PagedListResult<OrderDto>> DisplaysAsync(string poNumber,int? pageIndex, int? pageSize);
	}
}
