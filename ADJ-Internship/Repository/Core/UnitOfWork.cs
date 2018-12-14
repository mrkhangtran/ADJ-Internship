using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ADJ.Common;
using ADJ.DataAccess;
using ADJ.DataModel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ADJ.Repository.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApplicationContext _appContext;

        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext dbContext, ApplicationContext appContext)
        {
            _dbContext = dbContext;
            _appContext = appContext;
        }

        public async Task SaveChangesAsync(ConcurrencyResolutionStrategy strategy = ConcurrencyResolutionStrategy.None)
        {
            PreSaveChanges();

            bool saveFailed;

            switch (strategy)
            {
                case ConcurrencyResolutionStrategy.None:
                    try
                    {
                        // https://stackoverflow.com/questions/4402586/optimisticconcurrencyexception-does-not-work-in-entity-framework-in-certain-situ
                        
var isRowVersionChanged = _dbContext.ChangeTracker.Entries()
                            .Any(x => x.Properties.Any(m => m.Metadata.Name == "RowVersion") && x.CurrentValues.GetValue<byte[]>("RowVersion") != null && !x.CurrentValues.GetValue<byte[]>("RowVersion").SequenceEqual(x.OriginalValues.GetValue<byte[]>("RowVersion")));
                        if (isRowVersionChanged)
                        {
                            throw new AppException("Concurrency") { IsDbConcurrencyUpdate = true };
                        }

                        await _dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        throw new AppException(ex.Message) { IsDbConcurrencyUpdate = true };
                    }
                    
                    break;
                case ConcurrencyResolutionStrategy.DatabaseWin:
                    do
                    {
                        saveFailed = false;

                        try
                        {
                            await _dbContext.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            saveFailed = true;

                            // Update the values of the Entity that failed to save from the store 
                            ex.Entries.Single().Reload();
                        }

                    } while (saveFailed);

                    break;
                case ConcurrencyResolutionStrategy.ClientWin:
                    do
                    {
                        saveFailed = false;
                        try
                        {
                            await _dbContext.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            saveFailed = true;

                            // Update original values from the database 
                            var entry = ex.Entries.Single();
                            entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                        }

                    } while (saveFailed);


                    break;
                default:
                    await _dbContext.SaveChangesAsync();
                    break;
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken, ConcurrencyResolutionStrategy strategy = ConcurrencyResolutionStrategy.None)
        {
            PreSaveChanges();

            bool saveFailed;

            switch (strategy)
            {
                case ConcurrencyResolutionStrategy.None:
                    try
                    {
                        // https://stackoverflow.com/questions/4402586/optimisticconcurrencyexception-does-not-work-in-entity-framework-in-certain-situ
                        var isRowVersionChanged = _dbContext.ChangeTracker.Entries()
                            .Any(x => x.Properties.Any(m => m.Metadata.Name == "RowVersion") && x.CurrentValues.GetValue<byte[]>("RowVersion") != null && !x.CurrentValues.GetValue<byte[]>("RowVersion").SequenceEqual(x.OriginalValues.GetValue<byte[]>("RowVersion")));
                        if (isRowVersionChanged)
                        {
                            throw new AppException("Concurrency") { IsDbConcurrencyUpdate = true };
                        }

                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        throw new AppException(ex.Message) { IsDbConcurrencyUpdate = true };
                    }

                    break;
                case ConcurrencyResolutionStrategy.DatabaseWin:
                    do
                    {
                        saveFailed = false;

                        try
                        {
                            await _dbContext.SaveChangesAsync(cancellationToken);
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            saveFailed = true;

                            // Update the values of the Entity that failed to save from the store 
                            ex.Entries.Single().Reload();
                        }

                    } while (saveFailed);

                    break;
                case ConcurrencyResolutionStrategy.ClientWin:
                    do
                    {
                        saveFailed = false;
                        try
                        {
                            await _dbContext.SaveChangesAsync(cancellationToken);
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            saveFailed = true;

                            // Update original values from the database 
                            var entry = ex.Entries.Single();
                            entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                        }

                    } while (saveFailed);


                    break;
                default:
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    break;
            }
        }

        public T Reload<T>(T item) where T : EntityBase
        {
            _dbContext.Entry(item).Reload();
            return item;
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        private void PreSaveChanges()
        {
            HandleAudit();
        }

        private void HandleAudit()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditable auditable)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedBy = _appContext.Principal.Username;
                        auditable.CreatedDateUtc = DateTime.UtcNow;
                    }
                    else
                    {
                        auditable.ModifiedBy = _appContext.Principal.Username;
                        auditable.ModifiedDateUtc = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
