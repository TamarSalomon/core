
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace myTask.Services
{
    public static class TokenService
    {
        private static readonly SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
        private static readonly string issuer = "https://tasks.com";
        public static SecurityToken GetToken(List<Claim> claims) =>
            new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

        public static TokenValidationParameters GetTokenValidationParameters() =>
            new()
            {
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

        public static string WriteToken(SecurityToken token) =>
            new JwtSecurityTokenHandler().WriteToken(token);
    }
}



