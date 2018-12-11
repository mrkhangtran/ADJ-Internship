using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADJ.DataModel.Core;

namespace ADJ.Repository.Core
{
    public interface IFluentQuery<TEntity> where TEntity : EntityBase
    {
        IFluentQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

        /// <summary>
        /// Orders by
        /// </summary>
        /// <param name="orderBy">Sort definition in the format "Field1 asc,Field2 desc,Field3 asc"</param>
        /// <returns></returns>
        IFluentQuery<TEntity> OrderBy(string orderBy);

        IFluentQuery<TEntity> Include(Func<IQueryable<TEntity>, IQueryable<TEntity>> include);

        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, int? maxItems = null);

        Task<List<TEntity>> SelectAsync(int? maxItems = null);

        Task<List<TEntity>> SelectPagingAsync(int page, int pageSize, out int totalCount, out int pageCount);
    }
}
