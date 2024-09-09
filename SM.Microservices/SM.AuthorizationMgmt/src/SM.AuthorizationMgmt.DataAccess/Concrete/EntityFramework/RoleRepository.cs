using SM.AuthorizationMgmt.DataAccess.Abstract;
using SM.AuthorizationMgmt.DataAccess.Contexts;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.DataAccess.EntityFramework;

namespace SM.AuthorizationMgmt.DataAccess.Concrete.EntityFramework
{
    public class RoleRepository : EfEntityRepositoryBase<Role, AuthorizationDbContext>, IRoleRepository
    {
        public RoleRepository(AuthorizationDbContext context) : base(context)
        {
        }
    }
}
