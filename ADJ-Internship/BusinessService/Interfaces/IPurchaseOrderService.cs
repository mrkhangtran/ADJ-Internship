using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ADJ.BusinessService.Dtos;
using ADJ.Common;

namespace ADJ.BusinessService.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<PagedListResult<PurchaseOrderDto>> ListPurchaseOrdersAsync(string searchTerm);
        Task<PurchaseOrderDto> CreateOrUpdatePurchaseOrderAsync(CreateOrUpdatePurchaseOrderRq rq);
        Task DeletePurchaseOrderAsync(int id);
    }
}
