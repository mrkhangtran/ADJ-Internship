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
  public class DCReceivedRepository : RepositoryBase<DCConfirmation>, IDCReceivedRepository
  {
    public DCReceivedRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    protected override Func<IQueryable<DCConfirmation>, IQueryable<DCConfirmation>> IncludeDependents => null;
  }
}
