using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADJ.Common;
using ADJ.DataModel.Core;

namespace ADJ.BusinessService.Core
{
    public interface IDataProvider<T> where T : EntityBase
    {
        Task<T> GetByIdAsync(int id);

        Task<PagedListResult<T>> ListAsync(Expression<Func<T, bool>> filter = null, string sortData = null, bool includeDependents = false, int? pageIndex = null, int? pageSize = null);
    }
}
