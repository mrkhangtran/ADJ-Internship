using ADJ.DataAccess;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace ADJ.Repository.Implementations
{
  public class ShipmentBookingRepository : RepositoryBase<Booking>, IShipmentBookingRepository
  {
    public ShipmentBookingRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }
    protected override Func<System.Linq.IQueryable<Booking>, System.Linq.IQueryable<Booking>> IncludeDependents => po => po.Include(x => x.Manifests);

  }
}
