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
}
