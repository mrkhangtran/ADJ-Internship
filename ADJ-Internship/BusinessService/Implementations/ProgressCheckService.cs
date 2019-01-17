using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

namespace ADJ.BusinessService.Implementations
{
  public class ProgressCheckService : ServiceBase, IProgressCheckService
  {
    private readonly IDataProvider<Order> _orderDataProvider;
    private readonly IOrderRepository _orderRepository;

    private readonly IOrderDetailRepository _orderdetailRepository;
    private readonly IDataProvider<OrderDetail> _orderdetailDataProvider;

    private readonly IDataProvider<ProgressCheck> _progresscheckDataProvider;
    private readonly IProgressCheckRepository _progresscheckRepository;

    public ProgressCheckService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
        IDataProvider<ProgressCheck> progresscheckDataProvider, IDataProvider<Order> orderDataProvider,
        IDataProvider<OrderDetail> orderdetailDataProvider, IProgressCheckRepository progresscheckRepository,
        IOrderRepository orderRepository, IOrderDetailRepository orderdetailRepository) : base(unitOfWork, mapper, appContext)
    {
      _orderDataProvider = orderDataProvider;
      _orderRepository = orderRepository;

      _orderdetailRepository = orderdetailRepository;
      _orderdetailDataProvider = orderdetailDataProvider;

      _progresscheckRepository = progresscheckRepository;
      _progresscheckDataProvider = progresscheckDataProvider;
    }
    public async Task<PagedListResult<ProgressCheckDto>> ListProgressCheckDtoAsync(int pageIndex = 1, int pageSize = 2, string PONumberSearch = null, string ItemSearch = null,
      string Vendor = null, string Factories = null, string Origins = null, string OriginPorts = null, string Depts = null, string Status = null)
    {
      List<ProgressCheckDto> progressCheckDTOs = new List<ProgressCheckDto>();
      Expression<Func<Order, bool>> All = x => x.Id > 0;
      if (PONumberSearch != null)
      {
        // add condition x.PO==POSearch
        Expression<Func<Order, bool>> filterPO = x => x.PONumber.Contains(PONumberSearch);
        //var test = a.And(a);
        All = All.And(filterPO);

      }
      if (Origins != null)
      {
        Expression<Func<Order, bool>> filterOrigin = x => x.Origin == Origins;
        All = All.And(filterOrigin);
      }
      if (OriginPorts != null)
      {
        // add condition x.port==port
        Expression<Func<Order, bool>> filterOriginPort = x => x.PortOfLoading == OriginPorts;
        All = All.And(filterOriginPort);
      }
      if (Factories != null)
      {
        Expression<Func<Order, bool>> filterFactories = x => x.Factory == Factories;
        All = All.And(filterFactories);
        //add condition x.fac == fac
      }
      if (Vendor != null)
      {
        Expression<Func<Order, bool>> filterSuppliers = x => x.Vendor == Vendor;
        All = All.And(filterSuppliers);
        //add condition x.supli==supli
      }
      if (Depts != null)
      {
        Expression<Func<Order, bool>> filterDepts = x => x.Department == Depts;
        All = All.And(filterDepts);
        //add condition x.depts==depts
      }
      if (Status != null)
      {
        Expression<Func<Order, bool>> filterStatus = x => x.OrderDetails.Where(p => p.Status.ToString() == Status).Count() > 0;
        All = All.And(filterStatus);
        //add condition x.depts==depts
      }
      if (ItemSearch != null)
      {
        Expression<Func<Order, bool>> filterItem = x => x.OrderDetails.Where(p => p.ItemNumber.Contains(ItemSearch)).Count() > 0;
        All = All.And(filterItem);
      }
      var GetPageResult = await _orderDataProvider.ListAsync(All, "PONumber", true, pageIndex, pageSize);
      List<Order> orders = GetPageResult.Items;
      foreach (var order in orders)
      {
        decimal POQuantity = 0;
        foreach (var orderDetail in order.OrderDetails)
        {
          POQuantity += orderDetail.Quantity;
        }
        ProgressCheck progressCheck = new ProgressCheck();
        ProgressCheck check = _progresscheckRepository.GetProgressCheckByOrderId(order.Id);
        if (check == null)
        {
          progressCheck.Id = 0;
          progressCheck.InspectionDate = DateTime.Now.Date;
          progressCheck.IntendedShipDate = DateTime.Now.Date;
          progressCheck.Complete = false;
          progressCheck.OrderId = order.Id;
        }
        else
        {
          progressCheck = check;
          if (progressCheck.EstQtyToShip != POQuantity)
          {
            progressCheck.Complete = false;
          }
        }
        ProgressCheckDto temp = new ProgressCheckDto()
        {
          Id = progressCheck.Id,
          Factory = order.Factory,
          PONumber = order.PONumber,
          ShipDate = order.ShipDate,
          InspectionDate = progressCheck.InspectionDate,
          IntendedShipDate = progressCheck.IntendedShipDate,
          Complete = progressCheck.Complete,
          POQuantity = POQuantity,
          EstQtyToShip = progressCheck.EstQtyToShip,
          Supplier = order.Vendor,
          ListOrderDetailDto = Mapper.Map<List<OrderDetailDto>>(order.OrderDetails),
          OrderId = order.Id,
          Origin = order.Origin,
          OriginPort = order.PortOfDelivery,
          Department = order.Department,
        };
        progressCheckDTOs.Add(temp);
      }
      PagedListResult<ProgressCheckDto> lstProChDto = new PagedListResult<ProgressCheckDto>
      {
        TotalCount = GetPageResult.TotalCount,
        PageCount = GetPageResult.PageCount,
        Items = progressCheckDTOs,
      };
      return lstProChDto;
    }
    public async Task<ProgressCheckDto> CreateOrUpdateProgressCheckAsync(ProgressCheckDto rq)
    {
      ProgressCheck entity = new ProgressCheck();
      //rq.ListOrderDetail = Mapper.Map<List<OrderDetail>>(rq.ListOrderDetailDto);
      if (rq.Id > 0)
      {
        entity = await _progresscheckRepository.GetByIdAsync(rq.Id, false);
        if (entity == null)
        {
          throw new AppException("Progress Check Not Found");
        }
        if (rq.selected == true)
        {
          entity.InspectionDate = rq.InspectionDate;
          entity.IntendedShipDate = rq.IntendedShipDate;
        }
        decimal temp = 0;

        foreach (var item in rq.ListOrderDetailDto)
        {
          OrderDetail orderDetail = await _orderdetailRepository.GetByIdAsync(item.Id, false);
          if (item.selected == true)
          {
            temp += item.ReviseQuantity;
            orderDetail.ReviseQuantity = item.ReviseQuantity;
            if (orderDetail.Quantity == item.ReviseQuantity)
            {
              orderDetail.Status = OrderStatus.AwaitingBooking;
            }
            _orderdetailRepository.Update(orderDetail);
          }
          else
          {
            temp += orderDetail.ReviseQuantity;
          }
        }
        entity.EstQtyToShip = temp;
        if (entity.EstQtyToShip == rq.POQuantity)
        {
          entity.Complete = true;
        }
        else
        {
          entity.Complete = false;
        }
        _progresscheckRepository.Update(entity);
      }
      else
      {
        entity.InspectionDate = rq.InspectionDate;
        entity.IntendedShipDate = rq.IntendedShipDate;
        entity.OrderId = rq.OrderId;
        decimal temp = 0;
        foreach (var item in rq.ListOrderDetailDto)
        {
          OrderDetail orderDetail = await _orderdetailRepository.GetByIdAsync(item.Id, false);
          if (item.selected == true)
          {
            temp += item.ReviseQuantity;
            orderDetail.ReviseQuantity = item.ReviseQuantity;
            if (orderDetail.Quantity == item.ReviseQuantity)
            {
              orderDetail.Status = OrderStatus.AwaitingBooking;
            }
            _orderdetailRepository.Update(orderDetail);
          }
          else
          {
            item.ReviseQuantity = orderDetail.ReviseQuantity;
          }
        }
        entity.EstQtyToShip = temp;
        if (entity.EstQtyToShip == rq.POQuantity)
        {
          entity.Complete = true;
        }
        else
        {
          entity.Complete = false;
        }
        _progresscheckRepository.Insert(entity);
      }

      await UnitOfWork.SaveChangesAsync();

      var rs = rq;
      rs.Complete = entity.Complete;
      rs.EstQtyToShip = entity.EstQtyToShip;
      rs.ShipDate = rq.ShipDate;
      rs.Supplier = rq.Supplier;
      rs.Factory = rq.Factory;
      rs.Id = entity.Id;
      return rs;
    }
    public async Task<GetItemSearchDto> SearchItem()
    {
      var lst = await _orderDataProvider.ListAsync();
      List<Order> orderModels = lst.Items;
      var suppliers = orderModels.Select(x => x.Vendor).Distinct();
      var origins = orderModels.Select(x => x.Origin).Distinct();
      var originports = orderModels.Select(x => x.PortOfLoading).Distinct();
      var factories = orderModels.Select(x => x.Factory).Distinct();
      var depts = orderModels.Select(x => x.Department).Distinct();
      List<string> status = Enum.GetNames(typeof(OrderStatus)).ToList();
      GetItemSearchDto getSearchItemDTO = new GetItemSearchDto()
      {
        Suppliers = suppliers,
        Origins = origins,
        OriginPorts = originports,
        Factories = factories,
        Depts = depts,
        Status = status,

      };
      return getSearchItemDTO;
    }
  }
}
