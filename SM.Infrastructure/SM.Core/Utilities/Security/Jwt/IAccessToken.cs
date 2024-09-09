using System.IdentityModel.Tokens.Jwt;

namespace SM.Core.Utilities.Security.Jwt
{
    public interface IAccessToken
    {
        string Token { get; set; }
        DateTime Expiration { get; set; }
        int NotBefore { get; set; }
        JwtSecurityToken JwtSecurityToken { get; set; }
        string UserName { get; set; }
        string FullName { get; set; }
        long SessionId { get; set; }

    }
}
