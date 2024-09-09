using Mapster;
using SM.AuthorizationMgmt.Business.Dtos.Authorizations;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.Domain.Dtos;
using SM.Core.Utilities.Security.Jwt;

namespace SM.AuthorizationMgmt.Business.Services.Authorizations
{
    public class AuthService : IAuthService
    {
        private readonly ITokenHelper _tokenHelper;

        public AuthService(ITokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        public AuthResultDto GenerateToken(User user)
        {
            var userDto = user.Adapt<UserDto>();
            var tokenDto = new TokenDto { User = userDto };

            var accessToken = _tokenHelper.CreateToken<AccessToken>(tokenDto);

            return new AuthResultDto(accessToken.Token, accessToken.Expiration);
        }
    }
}
