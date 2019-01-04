using ADJ.DataAccess;
using ADJ.DataModel.DeliveryTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADJ.Repository.Implementations
{
  public class DCBookingRepository : RepositoryBase<DCBooking>, IDCBookingRepository
  {
    public DCBookingRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    protected override Func<IQueryable<DCBooking>, IQueryable<DCBooking>> IncludeDependents => throw new NotImplementedException();
  }
}
