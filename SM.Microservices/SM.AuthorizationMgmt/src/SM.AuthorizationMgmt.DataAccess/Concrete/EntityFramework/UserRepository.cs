using SM.AuthorizationMgmt.DataAccess.Abstract;
using SM.AuthorizationMgmt.DataAccess.Contexts;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.DataAccess.EntityFramework;

namespace SM.AuthorizationMgmt.DataAccess.Concrete.EntityFramework
{
    public class UserRepository : EfEntityRepositoryBase<User, AuthorizationDbContext>, IUserRepository
    {
        public UserRepository(AuthorizationDbContext context) : base(context)
        {
        }
    }
}
