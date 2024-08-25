using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TradeCore.AuthService.Domain.AppCustomerAggregate;
using TradeCore.AuthService.Helpers;
using TradeCore.AuthService.Helpers.HelperModels;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAuthRepositoryAggregate;
using TradeCore.AuthService.Security.Encyption;
using TradeCore.AuthService.Security.Jwt;

namespace TradeCore.AuthService.Repository.RepositoryAggregate.RepositoryAuthAggregate
{
    public class TokenRepository : GenericRepository<AppCustomer>, ITokenRepository
    {
        private IConfiguration _config;
        private DateTime _accessTokenExpiration;
        private readonly TokenOptions _tokenOptions;
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(AuthDbContext context, IConfiguration config, ILogger<TokenRepository> logger) : base(context,logger)
        {
            _config = config;
            _tokenOptions = _config.GetSection("TokenOptions").Get<TokenOptions>();
            _logger = logger;

        }
        public string DecodeToken(string input)
        {
            var handler = new JwtSecurityTokenHandler();
            if (input.StartsWith("Bearer "))
                input = input.Substring("Bearer ".Length);
            return handler.ReadJwtToken(input).ToString();
        }
        public AppToken GenerateAccessToken(AppCustomer user)
        {
            _accessTokenExpiration = DateTimeHelper.DateTimeUtcTimeZone().AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);
 
            return new AppToken()
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, AppCustomer user,SigningCredentials signingCredentials)
        {
            try
            {
                var jwt = new JwtSecurityToken(

                        issuer: tokenOptions.Issuer,
                        audience: tokenOptions.Audience,
                        expires: _accessTokenExpiration,
                        notBefore: DateTimeHelper.DateTimeUtcTimeZone(),
                        claims: SetClaims(user),
                        signingCredentials: signingCredentials
                );

            return jwt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateJwtSecurityToken");
                return new JwtSecurityToken();
            }
        }
        private IEnumerable<Claim> SetClaims(AppCustomer user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            
            if (!string.IsNullOrEmpty(user.SessionId))
            claims.Add(new Claim(ClaimTypes.SerialNumber, user.SessionId.ToString()));
            if (!string.IsNullOrEmpty(user.Name))
            claims.Add(new Claim(ClaimTypes.Name, user.Name.ToString()));
            
            claims.Add(new Claim(ClaimTypes.Role, user.AuthenticationProviderType));


            return claims;
        }
    }
}
