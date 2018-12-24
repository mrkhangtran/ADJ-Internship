using ADJ.DataAccess;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADJ.Repository.Implementations
{
  public class ShipmentBookingRepository : RepositoryBase<Booking>, IShipmentBookingRepository
  {
    public ShipmentBookingRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }

    protected override Func<IQueryable<Booking>, IQueryable<Booking>> IncludeDependents => throw new NotImplementedException();
  }
}
