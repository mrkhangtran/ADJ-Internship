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
	public class ManifestRepository : RepositoryBase<Manifest>, IManifestRepository
	{
		public ManifestRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}

		//protected override Func<IQueryable<Manifest>, IQueryable<Manifest>> IncludeDependents =>
				//con => con.Include(x => x.Container);


		protected override Func<IQueryable<Manifest>, IQueryable<Manifest>> IncludeDependents => ma => ma.Include(x => x.Booking);

		public Manifest GetManifestByBookingId(int id)
		{
			return DbSet.SingleOrDefault(p => p.BookingId == id);
		}
	}
}
