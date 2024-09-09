using SM.AuthorizationMgmt.DataAccess.Abstract;
using SM.AuthorizationMgmt.DataAccess.Contexts;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.DataAccess.EntityFramework;

namespace SM.AuthorizationMgmt.DataAccess.Concrete.EntityFramework
{
    public class UserRoleRepository : EfEntityRepositoryBase<UserRole, AuthorizationDbContext>, IUserRoleRepository
    {
        public UserRoleRepository(AuthorizationDbContext context) : base(context)
        {
        }
    }
}
