using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Application.Abstractions
{
    public interface IJwtTokenManager
    {
        public string CreateNewToken(User? user = null, List<Claim>? additionalClaims = null);
        public string CreateToken(List<Claim>? additionalClaims = null);
        public bool ValidateToken(string? token = "");
        public (ClaimsPrincipal?, SecurityToken?) GetPrincipalFromToken(string? token = "");
        public string GetHeaderToken();
        public string GetValueFromToken(string token, string key);
        public string CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
