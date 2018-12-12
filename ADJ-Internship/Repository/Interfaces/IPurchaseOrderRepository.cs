using System;
using System.Collections.Generic;
using System.Text;
using ADJ.DataModel;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;

namespace ADJ.Repository.Interfaces
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
    }

    public interface IOrderRepository : IRepository<Order>
    {
    }

    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
    }
}
