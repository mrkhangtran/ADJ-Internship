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
	public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
	{
		public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}

		protected override Func<IQueryable<Booking>, IQueryable<Booking>> IncludeDependents =>
		con => con.Include(x => x.Manifests);
	}
}

