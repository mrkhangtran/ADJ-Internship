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
	public class ContainerRepository : RepositoryBase<Container>, IContainerRepository
	{
		public ContainerRepository(ApplicationDbContext dbContext) : base(dbContext)
		{

		}
		protected override Func<IQueryable<Container>, IQueryable<Container>> IncludeDependents => ma => ma.Include(x => x.Manifests);
	}
}

