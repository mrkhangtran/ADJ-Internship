using ADJ.DataAccess;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADJ.Repository.Implementations
{
    public class ProgressCheckRepository : RepositoryBase<ProgressCheck>, IProgressCheckRepository
    {
        public ProgressCheckRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        protected override Func<IQueryable<ProgressCheck>, IQueryable<ProgressCheck>> IncludeDependents => throw new NotImplementedException();
    }
}
