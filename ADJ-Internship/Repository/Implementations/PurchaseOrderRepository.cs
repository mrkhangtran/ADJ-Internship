using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADJ.DataAccess;
using ADJ.DataModel;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ADJ.Repository.Implementations
{
    public class PurchaseOrderRepository : RepositoryBase<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override Func<IQueryable<PurchaseOrder>, IQueryable<PurchaseOrder>> IncludeDependents =>
            po => po.Include(x => x.Items);  
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override Func<IQueryable<Order>, IQueryable<Order>> IncludeDependents =>
            po => po.Include(x => x.orderDetails);
    }

    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override Func<IQueryable<OrderDetail>, IQueryable<OrderDetail>> IncludeDependents =>
            po => po.Include(x => x.OrderId);
    }
}
