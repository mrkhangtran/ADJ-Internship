using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.Repository.Interfaces
{
  public interface IOrderRepository : IRepository<Order>
  {
    Task<int> GetLastOrderId();
  }
}
