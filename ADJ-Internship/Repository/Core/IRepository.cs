using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADJ.DataModel.Core;

namespace ADJ.Repository.Core
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> GetByIdAsync(int id, bool includeDependents);

        void Insert(TEntity entity);

        void InsertRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Delete(int id);

        void Delete(TEntity entity);

        IFluentQuery<TEntity> Query(IQueryObject<TEntity> queryObject, bool includeDependents);

        IFluentQuery<TEntity> Query(Expression<Func<TEntity, bool>> query, bool includeDependents);

        IFluentQuery<TEntity> Query(bool includeDependents);
    }
}
