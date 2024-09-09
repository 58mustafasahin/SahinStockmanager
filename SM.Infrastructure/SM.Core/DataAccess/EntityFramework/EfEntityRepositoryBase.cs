using Microsoft.EntityFrameworkCore;
using SM.Core.Domain.Entities;
using System.Linq.Expressions;

namespace SM.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext>
            : EfEntityReadRepositoryBase<TEntity, TContext>, IEntityRepository<TEntity>
            where TEntity : class, IEntity
            where TContext : DbContext, IMsContext
    {
        public EfEntityRepositoryBase(TContext context) : base(context)
        {
        }

        public TEntity Add(TEntity entity)
        {
            //SetInsertedDefaultFields(entity);
            //SetUpdatedDefaultFields(entity);
            return Context.Add(entity).Entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            //SetInsertedDefaultFields(entities);
            await Context.AddRangeAsync(entities);
        }

        public TEntity Update(TEntity entity)
        {
            Context.Update(entity);
            return entity;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            //SetUpdatedDefaultFields(entities);
            Context.UpdateRange(entities);
        }

        public void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            Context.RemoveRange(entities);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Transactional operations is prohibited when working with InMemoryDb!
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="successAction"></param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        public TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null, Action<Exception> exceptionAction = null)
        {
            var result = default(TResult);
            try
            {
                if (Context.Database.ProviderName.EndsWith("InMemory"))
                {
                    result = action();
                    SaveChanges();
                }
                else
                {
                    using (var tx = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            result = action();
                            SaveChanges();
                            tx.Commit();
                        }
                        catch (Exception)
                        {
                            tx.Rollback();
                            throw;
                        }
                    }
                }
                successAction?.Invoke();
            }
            catch (Exception ex)
            {
                if (exceptionAction == null)
                {
                    throw;
                }
                exceptionAction(ex);
            }
            return result;
        }

        public TEntity Create(TEntity entity)
        {
            var newEntry = Context.Set<TEntity>().Add(entity);
            return newEntry.Entity;
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public void CreateAndSave(IList<TEntity> entities)
        {

            Context.Set<TEntity>().AddRange(entities);
            Context.SaveChanges();
        }

        public TEntity CreateAndSave(TEntity entity)
        {
            Create(entity);
            Context.SaveChanges();
            return entity;
        }

        public async Task<TEntity> CreateAndSaveAsync(TEntity entity)
        {
            await CreateAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public void Delete(int id)
        {
            var entityToDelete = Context.Set<TEntity>().Find(id);
            Delete(entityToDelete);
        }

        public void Delete(Expression<Func<TEntity, bool>> filter)
        {
            var entitiesToDelete = Filter(filter);
            Context.Set<TEntity>().RemoveRange(entitiesToDelete);
        }

        public void HardDeleteAndSave(TEntity entity)
        {
            Delete(entity);
            Context.SaveChanges();
        }

        public void HardDeleteAndSave(IList<TEntity> entityList)
        {
            DeleteRange(entityList);
            Context.SaveChanges();
        }

        public void UpdateRange(IList<TEntity> entities)
        {
            if (entities.Count > 0)
            {
                Context.Set<TEntity>().UpdateRange(entities);
            }
        }

        public void UpdateAndSave(TEntity entity)
        {
            Update(entity);
            Context.SaveChanges();
        }

        public async Task UpdateAndSaveAsync(TEntity entity)
        {
            Update(entity);
            await Context.SaveChangesAsync();
        }

        public void UpdateAndSave(IList<TEntity> entityList)
        {
            UpdateRange(entityList);
            Context.SaveChanges();
        }
    }
}

