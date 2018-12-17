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
        /*Task<PagedListResult<PurchaseOrderDto>> ListPurchaseOrdersAsync(string searchTerm);
        Task<PurchaseOrderDto> CreateOrUpdatePurchaseOrderAsync(CreateOrUpdatePurchaseOrderRq rq);
        Task DeletePurchaseOrderAsync(int id);*/

        Task<PagedListResult<OrderDTO>> ListOrderAsync(string searchTerm);
        Task<OrderDTO> CreateOrUpdateOrderAsync(OrderDTO orderDTO);
        Task<OrderDetailDTO> CreateOrUpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO);
        Task<List<OrderDetailDTO>> DeleteOrderDetailAsync(List<OrderDetailDTO> orderDetailsView, int orderId);
        Task<bool> UniquePONumAsync(string PONumber, int? id);
        Task<bool> UniqueItemNumAsync(string itemNum, int? id);
        Task<OrderDTO> GetOrderByPONumber(string poNumber);
    }
}
