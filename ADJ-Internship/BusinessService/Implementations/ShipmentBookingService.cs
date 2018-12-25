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
    private readonly IDataProvider<Order> _orderDataProvider;

    private readonly IDataProvider<OrderDetail> _orderDetailDataProvider;

    private readonly IDataProvider<Booking> _bookingDataProvider;
    private readonly IShipmentBookingRepository _bookingRepository;

    private readonly int pageSize;

    public ShipmentBookingService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
      IDataProvider<OrderDetail> poDetailDataProvider, IDataProvider<Order> poDataProvider,
      IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository bookingRepository) : base(unitOfWork, mapper, appContext)
    {
      _orderDetailDataProvider = poDetailDataProvider;
      _orderDataProvider = poDataProvider;
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

      if (status != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.Status.ToString() == status;
        All = All.And(filter);
      }

      PagedListResult<OrderDetail> result = await _orderDetailDataProvider.ListAsync(All, null, true);

      return Mapper.Map<List<OrderDetailDTO>>(result.Items);
    }

    public async Task<List<ShipmentResultDtos>> ConvertToResultAsync(List<OrderDetailDTO> input)
    {
      List<ShipmentResultDtos> result = new List<ShipmentResultDtos>();

      foreach (var item in input)
      {
        //map OrderDetailDTO to ShipmentResult
        //get info from Order
        ShipmentResultDtos output = new ShipmentResultDtos();
        int OrderId = item.OrderId;
        PagedListResult<Order> order = await _orderDataProvider.ListAsync(x => x.Id == OrderId, null, false);
        output.PONumber = order.Items[0].PONumber;
        output.Vendor = order.Items[0].Vendor;
        output.POShipDate = order.Items[0].LatestShipDate;

        //get info from OrderDetail
        output.ItemNumber = item.ItemNumber;
        output.Quantity = item.Quantity;
        output.BookingQuantity = item.ReviseQuantity;
        output.Status = item.Status;
        output.OrderId = item.OrderId;

        //add item to result
        result.Add(output);
      }

      return result;
    }

    public async Task<List<ShipmentBookingDtos>> CreateOrUpdateBookingAsync(ShipmentBookingDtos booking)
    {
      List<Booking> result = new List<Booking>();

      List<ShipmentBookingDtos> bookings = ConvertToBookingList(booking);

      foreach (var item in bookings)
      {
        Booking entity;
        if (item.Status == OrderStatus.BookingMade)
        {
          entity = await GetBookingByItemNumber(item.Item);
          if (entity == null)
          {
            throw new AppException("Booking Not Found");
          }

          item.Id = entity.Id;
          item.RowVersion = entity.RowVersion.ToString();

          entity = Mapper.Map(booking, entity);

          _bookingRepository.Update(entity);
        }
        else
        {
          entity = Mapper.Map<Booking>(item);

          _bookingRepository.Insert(entity);
          result.Add(entity);
        }
      }

      await UnitOfWork.SaveChangesAsync();

      var rs = Mapper.Map<List<ShipmentBookingDtos>>(result);
      return rs;
    }

    public List<ShipmentBookingDtos> ConvertToBookingList(ShipmentBookingDtos input)
    {
      List<ShipmentBookingDtos> result = new List<ShipmentBookingDtos>();

      foreach (var item in input.OrderDetails)
        if (item.Selected)
        {
          {
            ShipmentBookingDtos output = new ShipmentBookingDtos();
            output.OrderId = item.OrderId;
            output.Item = item.ItemNumber;
            output.Quantity = item.Quantity;
            output.Cartons = item.Cartons;
            output.Cube = item.Cube;
            output.PackType = item.PackType;
            output.Status = item.Status;

            output.PortOfLoading = input.PortOfLoading;
            output.PortOfDelivery = input.PortOfDelivery;
            output.Carrier = input.Carrier;
            output.Mode = input.Mode;
            output.ETD = input.ETD;
            output.ETA = input.ETA;

            result.Add(output);
          }
        }

      return result;
    }

    public async Task<Booking> GetBookingByItemNumber(string itemNumber)
    {
      List<Booking> result = await _bookingRepository.Query(x => x.ItemNumber == itemNumber, false).SelectAsync();

      return result[0];
    }
  }
}
