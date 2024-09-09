using System.IdentityModel.Tokens.Jwt;

namespace SM.Core.Utilities.Security.Jwt
{
    public class AccessToken : IAccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public int NotBefore { get; set; }
        public List<string> Claims { get; set; }
        public JwtSecurityToken JwtSecurityToken { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public long SessionId { get; set; }
    }
}
