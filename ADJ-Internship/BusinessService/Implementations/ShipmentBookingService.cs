using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.OrderTrack;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
  public class ShipmentBookingService : ServiceBase, IShipmentBookingService
  {
    private readonly IDataProvider<OrderDetail> _orderDetailDataProvider;
    private readonly IOrderDetailRepository _orderDetailRepository;

    private readonly IDataProvider<Booking> _bookingDataProvider;
    private readonly IShipmentBookingRepository _bookingRepository;

    private readonly int pageSize;

    public ShipmentBookingService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
      IDataProvider<OrderDetail> poDetailDataProvider, IOrderDetailRepository poDetailRepository, 
      IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository bookingRepository) : base(unitOfWork, mapper, appContext)
    {
      _orderDetailDataProvider = poDetailDataProvider;
      _orderDetailRepository = poDetailRepository;
      _bookingDataProvider = bookingDataProvider;
      _bookingRepository = bookingRepository;
      pageSize = 6;
    }

    public async Task<List<OrderDetailDTO>> ListShipmentFilterAsync(int? page, string origin = null, string originPort = null, string mode = null, string warehouse = null, 
      string status = null, string vendor = null, string poNumber = null, string itemNumber = null)
    {
      if (page == null) { page = 1; }

      ShipmentBookingDtos shipmentFilter = new ShipmentBookingDtos();
      Expression<Func<OrderDetail, bool>> All = x => x.Id > 0;

      if (origin != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.Order.Origin == origin;
        All = All.And(filter);
      }

      if (originPort != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.Order.PortOfLoading == originPort;
        All = All.And(filter);
      }

      if (mode != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.Order.Mode == mode;
        All = All.And(filter);
      }

      if (vendor != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.Order.Vendor == vendor;
        All = All.And(filter);
      }

      if (poNumber != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.Order.PONumber == poNumber;
        All = All.And(filter);
      }

      if (warehouse != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.Warehouse == warehouse;
        All = All.And(filter);
      }

      if (itemNumber != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.ItemNumber == itemNumber;
        All = All.And(filter);
      }

      PagedListResult<OrderDetail> result = await _orderDetailDataProvider.ListAsync(All, null, true);

      //status filter

      return Mapper.Map<List<OrderDetailDTO>>(result.Items);
    }

    public async Task<List<string>> ListPortsAsync()
    {
      //var test = _orderDetailRepository.

      //List<string> result;

      //return result;
      return null;
    }
  }
}
