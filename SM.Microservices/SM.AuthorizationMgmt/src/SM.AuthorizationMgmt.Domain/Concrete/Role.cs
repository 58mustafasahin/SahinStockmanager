using SM.Core.Domain.Entities;

namespace SM.AuthorizationMgmt.Domain.Concrete
{
    public class Role : EntityDefinition
    {
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
