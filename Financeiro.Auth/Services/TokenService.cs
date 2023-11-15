using Financeiro.Auth.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Financeiro.Auth.Services
{
    public class TokenService : ITokenService
    {
        private const string SECRET = "fedaf7d8863b48e197b9287d492b708e"; // TODO: parâmetro

        public string GerarToken(string identifier, string usuarioId, string nomeUsuario, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, identifier),
                    new Claim(ClaimTypes.Name, usuarioId),
                    new Claim(ClaimTypes.GivenName, nomeUsuario),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public long? ObterIdentifierDoToken(string token)
        {
            var principal = ObterPrincipalDoTokenExpirado(token);

            if (principal == null)
                return null;

            var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (id == null)
                return null;

            return long.Parse(id);
        }

        private ClaimsPrincipal? ObterPrincipalDoTokenExpirado(string token)
        {
            var key = Encoding.ASCII.GetBytes(SECRET);
            var signingKey = new SymmetricSecurityKey(key);

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = false // não valida expiração
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
    }
}