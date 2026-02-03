using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Auth.Configs;
using Infrastructure.Auth.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Infrastructure.Auth.Managers
{
    public class JwtTokenManager(IConfiguration config, 
        IOptions<JWTAuth> options, 
        IHttpContextAccessor httpContextAccessor) : IJwtTokenManager
    {
        public string CreateNewToken(User? user = null, List<Claim>? claims = null)
        {
            claims = JwtHelper.GetClaims(claims);
            if (user != null)
            {
                List<Claim> userClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Surname, user.Name ?? ""),
                    new Claim(ClaimTypes.Email, user.Email.ToLower()),
                    new Claim(ClaimTypes.Role, user.Role.ToLower()),
                };
                claims.AddRange(userClaims);
            }
            string jwt = CreateToken(claims);
            return jwt;
        }

        public string CreateToken(List<Claim>? claims = null)
        {
            var tokenExpiringAt = DateTime.UtcNow.AddMinutes(15);
            if (claims is null) claims = JwtHelper.GetClaims(claims);
            if (!claims.Any(c => c.Type == ClaimTypes.Expiration))
            {
                claims.Add(new Claim(ClaimTypes.Expiration, tokenExpiringAt.ToString() ?? DateTime.UtcNow.AddMinutes(15).ToString()));
            }  
            var key = JwtHelper.GetSymmetricSecurityKey(options.Value, config);         
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                expires: tokenExpiringAt,
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public bool ValidateToken(string? token = "")
        {
            try
            {
                //var principal = handler.ValidateToken(token, parameters, out var validatedToken);
                var (principal, validatedToken) = GetPrincipalFromToken(token);
                
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase)) //StringComparison.InvariantCultureIgnoreCase
                {
                    return false;
                }

                if (principal is null || jwtToken is null)
                    return false;

                return jwtToken.ValidTo <= DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }

        public (ClaimsPrincipal?, SecurityToken?) GetPrincipalFromToken(string? token = "")
        {
            if (string.IsNullOrEmpty(token)) token = GetHeaderToken();
            var key = JwtHelper.GetSymmetricSecurityKey(options.Value, config);
            var _baseParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = options.Value.Issuer ?? "",
                ValidAudience = options.Value.Audience ?? "",
                // IMPORTANT: we’ll override ValidateLifetime per call
                ClockSkew = TimeSpan.Zero
            };

            var parameters = _baseParams.Clone();
            parameters.ValidateLifetime = false; // allow expired
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var principal = handler.ValidateToken(token, parameters, out var validatedToken);
                return (principal, validatedToken);
            }
            catch
            {
                return (null, null);
            }
        }

        public string GetHeaderToken()
        {
            var token = string.Empty;
            if (httpContextAccessor.HttpContext is not null)
                token = httpContextAccessor.HttpContext.Request.Headers.Authorization
                .FirstOrDefault()?
                .Replace("Bearer ", "") ?? "";
            return token;
        }

        public string GetHeaderRefreshToken()
        {
            var refreshToken = string.Empty;
            if (httpContextAccessor.HttpContext is not null)
                refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"] ?? "";
            return refreshToken;
        }

        public string GetValueFromToken(string token, string key) => JwtHelper.GetValueFromToken(token, key);
        public string CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) => PasswordHelper.HashWithHMACSHA512(password, out passwordHash, out passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) => PasswordHelper.VerifyWithHMACSHA512(password, passwordHash, passwordSalt);
    }
}
