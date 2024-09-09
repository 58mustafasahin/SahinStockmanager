using SM.Core.Domain.Entities;
using SM.Core.Utilities.Paging;
using System.Linq.Expressions;

namespace SM.Core.DataAccess
{
    public interface IEntityReadRepository<T>
        where T : class, IEntity
    {
        IEnumerable<T> GetList(Expression<Func<T, bool>> expression = null);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression = null);
        Task<PagedList<T>> GetPagedListAsync(PaginationQuery paginationQuery, Expression<Func<T, bool>> expression = null);
        Task<PagedList<T>> GetPagedListAsync(IQueryable<T> query, PaginationQuery paginationQuery);
        T Get(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> Query();
        bool Exists(Expression<Func<T, bool>> expression = null);
        Task<int> GetCountAsync(Expression<Func<T, bool>> expression = null);
        int GetCount(Expression<Func<T, bool>> expression = null);
        IQueryable<T> All();
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, params Func<IQueryable<T>, IQueryable<T>>[] includes);
        Task<T> FilterAsync(Expression<Func<T, bool>> filter, params Func<IQueryable<T>, IQueryable<T>>[] includes);
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, params Func<IQueryable<T>, IQueryable<T>>[] includes);
    }
}
