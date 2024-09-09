using Microsoft.EntityFrameworkCore;
using SM.Core.Domain.Entities;
using SM.Core.Utilities.Paging;
using System.Linq.Expressions;

namespace SM.Core.DataAccess.EntityFramework
{
    public class EfEntityReadRepositoryBase<TEntity, TContext>
            : IEntityReadRepository<TEntity>
            where TEntity : class, IEntity
            where TContext : DbContext
    {

        public EfEntityReadRepositoryBase(TContext context)
        {
            Context = context;
        }

        protected TContext Context { get; }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Query().FirstOrDefault(expression);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().AsQueryable().FirstOrDefaultAsync(expression);
        }

        private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null ? Query() : Query().Where(expression);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null)
        {
            return GetQuery(expression).AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null ? await Context.Set<TEntity>().ToListAsync() :
                 await Context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync(PaginationQuery paginationQuery, Expression<Func<TEntity, bool>> expression = null)
        {
            return await GetPagedListAsync(GetQuery(expression), paginationQuery);
        }

        public async Task<PagedList<TEntity>> GetPagedListAsync(IQueryable<TEntity> query, PaginationQuery paginationQuery)
        {
            if (paginationQuery == null || paginationQuery.PageSize < 1 || paginationQuery.PageNumber < 1)
            {
                return new PagedList<TEntity>(await query.ToListAsync(), query.Count(), 1, 0);
            }
            var items = await query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
                .Take(paginationQuery.PageSize)
                .ToListAsync();
            return new PagedList<TEntity>(items, query.Count(), paginationQuery.PageNumber, paginationQuery.PageSize);
        }

        public IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }

        public bool Exists(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
                return Query().AsNoTracking().Any();
            else
                return Query().AsNoTracking().Any(expression);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return await GetQuery(expression).CountAsync();
        }

        public int GetCount(Expression<Func<TEntity, bool>> expression = null)
        {
            return GetQuery(expression).Count();
        }

        public IQueryable<TEntity> All()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        private IQueryable<TEntity> IncludeQuery(IQueryable<TEntity> query, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
        {
            foreach (var include in includes)
            {
                query = include(query);
            }
            return query;
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().Where(filter).AsQueryable();
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
        {
            IQueryable<TEntity> query = IncludeQuery(All(), includes).Where(filter);
            int skipCount = index * size;
            query = skipCount == 0 ? query.Take(size) : query.Skip(skipCount).Take(size);
            total = query.Count();
            return query;
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
        {
            IQueryable<TEntity> query = IncludeQuery(All(), includes);
            return query.Where(filter);
        }

        public async Task<TEntity> FilterAsync(Expression<Func<TEntity, bool>> filter, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().Where(filter);
            return await IncludeQuery(query, includes).FirstOrDefaultAsync();
        }
    }
}
