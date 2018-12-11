using System;
using System.Linq.Expressions;
using ADJ.DataModel.Core;

namespace ADJ.Repository.Core
{
    public interface IQueryObject<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Returns the query expression
        /// </summary>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> Query();

        Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query);

        Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query);

        Expression<Func<TEntity, bool>> And(IQueryObject<TEntity> queryObject);

        Expression<Func<TEntity, bool>> Or(IQueryObject<TEntity> queryObject);
    }
}
