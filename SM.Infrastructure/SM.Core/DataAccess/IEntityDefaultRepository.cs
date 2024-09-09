using SM.Core.Domain.Entities;

namespace SM.Core.DataAccess
{
    public interface IEntityDefaultRepository<T> : IEntityRepository<T>
        where T : class, IEntityDefault
    {
    }
}
