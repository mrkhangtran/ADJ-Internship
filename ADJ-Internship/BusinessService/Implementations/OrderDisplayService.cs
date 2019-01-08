using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using FluentValidation;

namespace ADJ.BusinessService.Implementations
{
  public class OrderDisplayService : ServiceBase, IOrderDisplayService
  {
    private readonly IDataProvider<Order> _orderDataProvider;
    private readonly IOrderRepository _orderRepository;


    public OrderDisplayService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Order> orderDataProvider, IOrderRepository orderRepository) : base(unitOfWork, mapper, appContext)
    {
      this._orderDataProvider = orderDataProvider;
      this._orderRepository = orderRepository;
    }

    public async Task<PagedListResult<OrderDTO>> DisplaysAsync(string poNumber, int? pageIndex, int? pageSize)
    {

      Expression<Func<Order, bool>> query = (p => p.Id > 0);
      if (poNumber != null)
      {
        query = (p => p.PONumber.Contains(poNumber));
      }
      string sortStr = "OrderDate DESC";
      var poResult = await _orderDataProvider.ListAsync(query, sortStr, true, pageIndex, pageSize);

      var pagedResult = new PagedListResult<OrderDTO>
      {
        TotalCount = poResult.TotalCount,
        PageCount = poResult.PageCount,
        CurrentFilter = poNumber,
        Items = Mapper.Map<List<OrderDTO>>(poResult.Items)
      };

      foreach (var item in pagedResult.Items)
      {
        item.POQuantity = TotalQuantity(item);
      }


      return pagedResult;

    }

    public float TotalQuantity(OrderDTO order)
    {
      float total = 0;

      foreach (var item in order.orderDetails)
      {
        total += item.Quantity;
      }

      return total;
    }

  }
}
