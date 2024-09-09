using SM.Core.Domain.Entities;

namespace SM.AuthorizationMgmt.Domain.Concrete
{
    public class UserRole : EntityDefault
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long RoleId { get; set; }
        public Role Role { get; set; }
    }
}
