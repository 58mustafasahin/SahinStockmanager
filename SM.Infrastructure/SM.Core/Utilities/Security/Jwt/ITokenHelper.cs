using SM.Core.Domain.Dtos;

namespace SM.Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        TAccessToken CreateToken<TAccessToken>(TokenDto tokenDto)
          where TAccessToken : IAccessToken, new();
        TAccessToken CreateChangePasswordToken<TAccessToken>(TokenDto tokenDto)
          where TAccessToken : IAccessToken, new();
        string GetCurrentToken();
        long GetUserIdByCurrentToken();
        long GetCurrentOrganisationId();
        string DecodeToken(string input);
    }
}
