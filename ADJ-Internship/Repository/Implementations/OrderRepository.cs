using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADJ.DataAccess;
using ADJ.DataModel;
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

		protected override Func<IQueryable<Order>, IQueryable<Order>> IncludeDependents => throw new NotImplementedException();
	}
}
