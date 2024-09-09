using SM.AuthorizationMgmt.Business.Dtos.Authorizations;
using SM.AuthorizationMgmt.Domain.Concrete;

namespace SM.AuthorizationMgmt.Business.Services.Authorizations
{
    public interface IAuthService
    {
        AuthResultDto GenerateToken(User user);
    }
}
