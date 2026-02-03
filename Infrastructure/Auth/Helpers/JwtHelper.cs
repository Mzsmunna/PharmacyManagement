using Infrastructure.Auth.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Auth.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(string secretKey, object? payload)
        {
            if (string.IsNullOrEmpty(secretKey)) return string.Empty;
            JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            if (payload != null)
            {
                foreach (PropertyInfo claimInfo in payload.GetType().GetProperties())
                {
                    claims.Add(new Claim(claimInfo.Name, claimInfo.GetValue(payload, null)?.ToString() ?? ""));
                }
            }          

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            SecurityToken securityToken = _tokenHandler.CreateToken(securityTokenDescriptor);
            string token = _tokenHandler.WriteToken(securityToken);
            return token;
        }

        public static string GetValueFromToken(string token, string key)
        {
            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            if (securityToken is null) return "";
            var claim = securityToken.Claims.FirstOrDefault(claim => claim.Type == key);
            return (claim != null) ? claim.Value : "";
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(JWTAuth options, IConfiguration config)
        {
            string secret = options.SecretKey ??
                            config.GetValue<string>("JwtSecretKey")
                            ?? throw new Exception("JWT secret key not provided.");
            return GetSymmetricSecurityKey(secret);
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string secret)
        {
            if (string.IsNullOrEmpty(secret)) throw new Exception("JWT secret key not provided.");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            return signingKey;
        }

        public static List<Claim> GetClaims(List<Claim>? additionalClaims = null)
        {
            var jti = Guid.NewGuid().ToString();
            List<Claim> claims = new List<Claim>();            
            if (additionalClaims != null && additionalClaims.Any())
            {
                if (!claims.Any(c => c.Type == JwtRegisteredClaimNames.Jti))
                {
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));
                }
                claims.AddRange(additionalClaims);
            }
            else claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));        
            return claims;
        }

        public static TokenValidationParameters GetTokenValidationParameters(SymmetricSecurityKey signingKey, JWTAuth? config)
        {
            return new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero, //TimeSpan.FromSeconds(30)
                    IssuerSigningKey = signingKey,
                    ValidIssuer = config?.Issuer,
                    ValidAudience = config?.Audience,
                };
        }
    }
}
