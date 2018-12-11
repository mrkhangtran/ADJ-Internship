using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ADJ.DataAccess;
using ADJ.DataModel.Core;
using Microsoft.EntityFrameworkCore;

namespace ADJ.Repository.Core
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        protected abstract Func<IQueryable<TEntity>, IQueryable<TEntity>> IncludeDependents { get; }

        protected RepositoryBase(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id, bool includeDependents)
        {
            var query = DbSet.Where(x => x.Id == id);
            if (includeDependents && IncludeDependents != null)
            {
                query = IncludeDependents(query);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity != null)
            {
                DbSet.Add(entity);
            }
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            if (entities != null)
            {
                var list = entities.ToList();
                DbSet.AddRange(list);
            }
        }

        public virtual void Update(TEntity entity)
        {
            if (entity != null)
            {
                DbSet.Update(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = new TEntity { Id = id };
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                return;

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
            DbSet.Remove(entity);
        }

        public IFluentQuery<TEntity> Query(bool includeDependents)
        {
            return new FluentQuery<TEntity>(this, includeDependents ? IncludeDependents : null);
        }

        public virtual IFluentQuery<TEntity> Query(IQueryObject<TEntity> queryObject, bool includeDependents)
        {
            return new FluentQuery<TEntity>(this, queryObject, includeDependents ? IncludeDependents : null);
        }

        public virtual IFluentQuery<TEntity> Query(Expression<Func<TEntity, bool>> query, bool includeDependents)
        {
            return new FluentQuery<TEntity>(this, query, includeDependents ? IncludeDependents : null);
        }

        internal virtual IQueryable<TEntity> GetQueryable()
        {
            return DbSet;
        }
    }
}
