using System;
using ADJ.DataAccess;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ADJ.Repository.Implementations
{
	public class OrderRepository : RepositoryBase<Order>, IOrderRepository
	{
		public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
		{

		}
		protected override Func<System.Linq.IQueryable<Order>, System.Linq.IQueryable<Order>> IncludeDependents => po => po.Include(x => x.OrderDetails);
	}
}
