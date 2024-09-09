using SM.Core.Domain.Entities;
using System.Linq.Expressions;

namespace SM.Core.DataAccess
{
    public interface IEntityRepository<T> : IEntityReadRepository<T>
        where T : class, IEntity
    {
        T Add(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null, Action<Exception> exceptionAction = null);
        T Create(T entity);
        Task<T> CreateAsync(T entity);
        T CreateAndSave(T entity);
        Task<T> CreateAndSaveAsync(T entity);
        void CreateAndSave(IList<T> entityList);
        void Delete(int id);
        void HardDeleteAndSave(T entity);
        void HardDeleteAndSave(IList<T> entityList);
        void Delete(Expression<Func<T, bool>> filter);
        void UpdateAndSave(T entity);
        void UpdateAndSave(IList<T> entityList);
        Task UpdateAndSaveAsync(T entity);

    }
}

