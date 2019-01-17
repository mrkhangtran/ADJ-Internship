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
    private readonly IOrderRepository _orderRepository;

    private readonly IDataProvider<OrderDetail> _orderDetailDataProvider;
    private readonly IOrderDetailRepository _orderDetailRepository;

    private readonly IDataProvider<Booking> _bookingDataProvider;
    private readonly IShipmentBookingRepository _bookingRepository;

    private readonly IProgressCheckRepository _progressCheckRepository;

    public ShipmentBookingService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
      IDataProvider<OrderDetail> poDetailDataProvider, IOrderDetailRepository orderdetailRepository,
      IDataProvider<Order> poDataProvider, IOrderRepository orderRepository,
      IDataProvider<Booking> bookingDataProvider, IShipmentBookingRepository bookingRepository,
      IProgressCheckRepository progressCheckRepository) : base(unitOfWork, mapper, appContext)
    {
      _orderDetailDataProvider = poDetailDataProvider;
      _orderDetailRepository = orderdetailRepository;

      _orderDataProvider = poDataProvider;
      _orderRepository = orderRepository;

      _bookingDataProvider = bookingDataProvider;
      _bookingRepository = bookingRepository;

      _progressCheckRepository = progressCheckRepository;
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

      Expression<Func<OrderDetail, bool>> All1 = x => x.Status == OrderStatus.AwaitingBooking;
      Expression<Func<OrderDetail, bool>> All2 = x => x.Status == OrderStatus.BookingMade;

      if (originPort != null)
      {
        Expression<Func<OrderDetail, bool>> filter1 = x => x.Order.PortOfLoading == originPort;
        All1 = All1.And(filter1);
        Expression<Func<OrderDetail, bool>> filter2 = x => x.Order.Bookings.Where(p => p.PortOfLoading == originPort).Count() > 0;
        All2 = All2.And(filter2);
      }

      if (mode != null)
      {
        Expression<Func<OrderDetail, bool>> filter = x => x.Order.Mode == mode;
        All1 = All1.And(filter);
        Expression<Func<OrderDetail, bool>> filter2 = x => x.Order.Bookings.Where(p => p.Mode == mode).Count() > 0;
        All2 = All2.And(filter2);
      }

      if (status == OrderStatus.AwaitingBooking.GetDescription<OrderStatus>())
      {
        All = All.And(All1);
      }
      else if (status == OrderStatus.BookingMade.GetDescription<OrderStatus>())
      {
        All = All.And(All2);
      }
      else
      {
        All = All.And(All1.Or(All2));
      }

      PagedListResult<OrderDetail> result = await _orderDetailDataProvider.ListAsync(All, null, true);

      return Mapper.Map<List<OrderDetailDTO>>(result.Items);
    }

    public async Task<List<ShipmentResultDtos>> UpdatePackType(List<ShipmentResultDtos> input)
    {
      List<ShipmentResultDtos> output = input;

      foreach (var item in output)
      {
        if (item.Status == OrderStatus.BookingMade)
        {
          Booking booking = await GetBookingByItemNumber(item.ItemNumber);
          item.PackType = booking.PackType;
        }
      }

      return output;
    }

    public async Task<List<ShipmentResultDtos>> ConvertToResultAsync(List<OrderDetailDTO> input)
    {
      List<ShipmentResultDtos> result = new List<ShipmentResultDtos>();

      foreach (var item in input)
      {
        //map OrderDetailDTO to ShipmentResult
        ShipmentResultDtos output = new ShipmentResultDtos();
        int OrderId = item.OrderId;
        PagedListResult<Order> order = await _orderDataProvider.ListAsync(x => x.Id == OrderId, null, false);
        List<Booking> booking = await _bookingRepository.Query(x => x.ItemNumber == item.ItemNumber, false).SelectAsync();
        List<ProgressCheck> progressCheck = await _progressCheckRepository.Query(x => x.OrderId == order.Items[0].Id, false).SelectAsync();

        //get info from Order
        output.PONumber = order.Items[0].PONumber;
        output.Vendor = order.Items[0].Vendor;
        //output.POShipDate = order.Items[0].ShipDate;
        output.POShipDate = progressCheck[0].IntendedShipDate;
        output.DeliveryDate = order.Items[0].DeliveryDate;

        //get info from OrderDetail
        output.ItemNumber = item.ItemNumber;
        output.Quantity = item.Quantity;
        output.BookingQuantity = item.ReviseQuantity;
        output.Status = item.Status;
        output.StatusDescription = item.Status.GetDescription<OrderStatus>();
        output.OrderId = item.OrderId;
        output.Cartons = item.Cartons;
        output.Cube = item.Cube;

        //get info from Booking
        if (booking.Count > 0)
        {
          output.ShipmentID = booking[0].ShipmentID;
        }

        //add item to result
        result.Add(output);
      }

      return result.OrderBy(p => p.ShipmentID).ThenBy(p => p.PONumber).ThenBy(p => p.ItemNumber).ToList();
    }

    public async Task<List<ShipmentBookingDtos>> CreateOrUpdateBookingAsync(ShipmentBookingDtos booking)
    {
      List<Booking> result = new List<Booking>();

      List<ShipmentBookingDtos> bookings = ConvertToBookingList(booking);

      PagedListResult<Booking> bookingDb = await _bookingDataProvider.ListAsync();

      foreach (var item in bookings)
      {
        Booking entity = new Booking();
        if (item.Status == OrderStatus.BookingMade)
        {
          foreach (var detail in bookingDb.Items)
          {
            if (detail.ItemNumber == item.ItemNumber)
            {
              entity = detail;
              break;
            }
          }

          item.Id = entity.Id;
          var temp = entity.RowVersion;
          entity = Mapper.Map(item, entity);
          entity.RowVersion = temp;

          _bookingRepository.Update(entity);
          result.Add(entity);
        }
        else
        {
          item.Status = OrderStatus.BookingMade;
          entity = Mapper.Map<Booking>(item);

          result.Add(entity);
          _bookingRepository.Insert(entity);
        }
      }

      //await UnitOfWork.SaveChangesAsync();

      var rs = Mapper.Map<List<ShipmentBookingDtos>>(result);
      return rs;
    }

    public List<ShipmentBookingDtos> ConvertToBookingList(ShipmentBookingDtos input)
    {
      List<ShipmentBookingDtos> result = new List<ShipmentBookingDtos>();

      Guid shipmentId = Guid.NewGuid();

      foreach (var item in input.OrderDetails)
        if (item.Selected)
        {
          {
            ShipmentBookingDtos output = new ShipmentBookingDtos();
            output.OrderId = item.OrderId;
            output.ItemNumber = item.ItemNumber;
            output.Quantity = item.Quantity;
            output.Cartons = item.Cartons;
            output.Cube = item.Cube;
            output.PackType = item.PackType;
            output.Status = item.Status;
            output.PONumber = item.PONumber;

            output.PortOfLoading = input.PortOfLoading;
            output.PortOfDelivery = input.PortOfDelivery;
            output.Carrier = input.Carrier;
            output.Mode = input.Mode;
            output.ETD = input.ETD;
            output.ETA = input.ETA;

            output.ShipmentID = shipmentId;

            result.Add(output);
          }
        }

      return result;
    }

    public async Task<ShipmentBookingDtos> ChangeItemStatus(ShipmentBookingDtos input)
    {
      foreach (var item in input.OrderDetails)
        if (item.Selected)
        {
          {
            List<Booking> booking = await _bookingRepository.Query(x => x.ItemNumber == item.ItemNumber, false).SelectAsync();
            if (booking.Count > 0)
            {
              item.ShipmentID = booking[0].ShipmentID;
            }

            item.Status = OrderStatus.BookingMade;
            item.StatusDescription = OrderStatus.BookingMade.GetDescription<OrderStatus>();

            List<OrderDetail> orderDetails = await _orderDetailRepository.Query(x => x.ItemNumber == item.ItemNumber, false).SelectAsync();
            orderDetails[0].Status = OrderStatus.BookingMade;

            _orderDetailRepository.Update(orderDetails[0]);

            //update Order info
            List<Order> order = await _orderRepository.Query(x => x.Id == item.OrderId, false).SelectAsync();
            if (order == null)
            {
              throw new AppException("Order not found.");
            }

            //if ((order[0].PortOfLoading != input.PortOfLoading) || (order[0].PortOfDelivery != input.PortOfDelivery) || (order[0].Mode != input.Mode))
            //{
            //  order[0].PortOfLoading = input.PortOfLoading;
            //  order[0].PortOfDelivery = input.PortOfDelivery;
            //  order[0].Mode = input.Mode;

            //  _orderRepository.Update(order[0]);
            //}
          }
        }

      await UnitOfWork.SaveChangesAsync();

      foreach (var item in input.OrderDetails)
        if (item.Selected)
        {
          {
            List<Booking> booking = await _bookingRepository.Query(x => x.ItemNumber == item.ItemNumber, false).SelectAsync();
            if (booking.Count > 0)
            {
              item.ShipmentID = booking[0].ShipmentID;
            }
          }
        }

      return input;
    }

    public async Task<Booking> GetBookingByItemNumber(string itemNumber)
    {
      List<Booking> result = await _bookingRepository.Query(x => x.ItemNumber == itemNumber, false).SelectAsync();

      return result[0];
    }
  }
}
