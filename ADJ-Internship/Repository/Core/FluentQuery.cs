using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADJ.DataModel.Core;
using Microsoft.EntityFrameworkCore;

namespace ADJ.Repository.Core
{
    public class FluentQuery<TEntity> : IFluentQuery<TEntity> where TEntity : EntityBase, new()
    {
        #region Private Fields

        private IQueryable<TEntity> _query;
        private readonly RepositoryBase<TEntity> _repositoryBase;
        private bool _isSorted;

        #endregion Private Fields

        #region Constructors

        public FluentQuery(RepositoryBase<TEntity> repositoryBase, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeDependents)
        {
            _repositoryBase = repositoryBase;
            _query = repositoryBase.GetQueryable();

            if (includeDependents != null)
            {
                _query = includeDependents(_query);
            }
        }

        public FluentQuery(RepositoryBase<TEntity> repositoryBase, IQueryObject<TEntity> queryObject, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeDependents)
            : this(repositoryBase, includeDependents)
        {
            _query = _query.Where(queryObject.Query());
        }

        public FluentQuery(RepositoryBase<TEntity> repositoryBase, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IQueryable<TEntity>> includeDependents)
            : this(repositoryBase, includeDependents)
        {
            _query = _query.Where(filter);
        }

        #endregion Constructors

        public IFluentQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _query = orderBy(_query);
            _isSorted = true;
            return this;
        }

        public IFluentQuery<TEntity> OrderBy(string orderBy)
        {
            _query = _query.OrderBy(orderBy);
            _isSorted = true;
            return this;
        }

        public IFluentQuery<TEntity> Include(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            _query = include(_query);
            return this;
        }

        public Task<List<TEntity>> SelectPagingAsync(int page, int pageSize, out int totalCount, out int pageCount)
        {
            if (!_isSorted)
            {
                _query = _query.OrderBy(x => x.Id);
            }

            totalCount = _query.Count();
            pageCount = (totalCount + pageSize - 1) / pageSize;
            return _query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<List<TEntity>> SelectAsync(int? maxItems = null)
        {
            if (maxItems.HasValue)
                return _query.Take(maxItems.Value).ToListAsync();

            return _query.ToListAsync();
        }

        public Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, int? maxItems = null)
        {
            if (maxItems.HasValue)
                return _query.Take(maxItems.Value).Select(selector).ToListAsync();

            return _query.Select(selector).ToListAsync();
        }
    }
}
