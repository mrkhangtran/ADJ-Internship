using ADJ.DataAccess;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.Repository.Implementations
{
  public class OrderRepository : RepositoryBase<Order>, IOrderRepository
  {
    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }
    protected override Func<System.Linq.IQueryable<Order>, System.Linq.IQueryable<Order>> IncludeDependents => po => po.Include(x => x.orderDetails);
    public async Task<int> GetLastOrderId()
    {
      return await DbSet.MaxAsync(x => x.Id);
    }
  }
}
