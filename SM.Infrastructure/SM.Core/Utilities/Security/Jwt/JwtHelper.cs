using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using SM.Core.Domain.Dtos;
using SM.Core.Extensions;
using SM.Core.Utilities.Security.Encryption;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SM.Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        private readonly TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        private DateTime _accessTokenNotBefore;

        public JwtHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            HttpContextAccessor = httpContextAccessor;
        }

        public IConfiguration Configuration { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public string DecodeToken(string input)
        {
            var handler = new JwtSecurityTokenHandler();
            if (input.StartsWith("Bearer "))
            {
                input = input.Substring("Bearer ".Length);
            }

            return handler.ReadJwtToken(input).ToString();
        }

        public TAccessToken CreateToken<TAccessToken>(TokenDto tokenDto)
            where TAccessToken : IAccessToken, new()
        {
            _accessTokenNotBefore = DateTime.Now;
            _accessTokenExpiration = _accessTokenNotBefore.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, tokenDto, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new TAccessToken()
            {
                Token = token,
                UserName = tokenDto.User.UserName,
                FullName = tokenDto.User.GetFullName(),
                Expiration = _accessTokenExpiration,
                NotBefore = Convert.ToInt32(jwt.Claims.FirstOrDefault(x => x.Type.Equals("nbf"))?.Value),
                SessionId = tokenDto.SessionId
            };
        }
        public TAccessToken CreateChangePasswordToken<TAccessToken>(TokenDto tokenDto)
            where TAccessToken : IAccessToken, new()
        {
            _accessTokenNotBefore = DateTime.UtcNow;
            _accessTokenExpiration = _accessTokenNotBefore.AddMinutes(2 * 24 * 60);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, tokenDto, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new TAccessToken()
            {
                Token = token,
                Expiration = _accessTokenExpiration,
                NotBefore = Convert.ToInt32(jwt.Claims.FirstOrDefault(x => x.Type.Equals("nbf"))?.Value),
                JwtSecurityToken = jwt,
                SessionId = tokenDto.SessionId
            };
        }

        public JwtSecurityToken CreateJwtSecurityToken(
            TokenOptions tokenOptions,
            TokenDto tokenDto,
            SigningCredentials signingCredentials
            )
        {
            var jwt = new JwtSecurityToken(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: _accessTokenNotBefore,
                claims: SetClaims(tokenDto),
                signingCredentials: signingCredentials);
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(TokenDto tokenDto)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(tokenDto.User.Id.ToString());
            claims.AddNameUniqueIdentifier(tokenDto.User.UserCode.ToString());

            if (!string.IsNullOrEmpty(tokenDto.User.GetFullName()))
            {
                claims.AddName($"{tokenDto.User.GetFullName()}");
            }
            return claims;
        }

        public string GetCurrentToken()
        {
            string token = HttpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault(w => w.Contains("Bearer"));
            return token.Replace("Bearer", "").Trim();
        }

        public long GetUserIdByCurrentToken()
        {
            string userId = HttpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
            long result = long.TryParse(userId, out result) ? result : 0;
            return result;
        }

        public long GetCurrentOrganisationId()
        {
            StringValues currenttOrganisationIdStr = "0";
            long currenttOrganisationId = 0;
            HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("CurrentOrganisationId",
               out currenttOrganisationIdStr);

            if (!long.TryParse(currenttOrganisationIdStr, out currenttOrganisationId))
            {
                currenttOrganisationId = 0;
            }
            return currenttOrganisationId;
        }
    }
}