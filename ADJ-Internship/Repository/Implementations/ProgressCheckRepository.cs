using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADJ.DataAccess;
using ADJ.DataModel;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ADJ.Repository.Implementations
{
    public class ProgressCheckRepository : RepositoryBase<ProgressCheck>, IProgressCheckRepository
    {
        public ProgressCheckRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override Func<IQueryable<ProgressCheck>, IQueryable<ProgressCheck>> IncludeDependents =>
            po => po.Include(x => x.OrderId);
    }
}
