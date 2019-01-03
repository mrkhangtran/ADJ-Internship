using ADJ.DataAccess;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADJ.Repository.Implementations
{
	public class ArriveOfDespatchRepository : RepositoryBase<ArriveOfDespatch>, IArriveOfDespatchRepository
	{
		public ArriveOfDespatchRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}

		protected override Func<IQueryable<ArriveOfDespatch>, IQueryable<ArriveOfDespatch>> IncludeDependents =>
			null;
	}
}


