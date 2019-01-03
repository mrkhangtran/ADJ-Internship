using ADJ.DataAccess;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.Repository.Implementations
{
  public class ConfirmArrivalRepository : RepositoryBase<CA>, IConfirmArrivalRepository
  {
    public ConfirmArrivalRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }
    protected override Func<System.Linq.IQueryable<CA>, System.Linq.IQueryable<CA>> IncludeDependents => null;
  }
}
