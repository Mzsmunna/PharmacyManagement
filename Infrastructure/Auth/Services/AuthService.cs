using Application.Abstractions;
using Application.Dtos;
using Application.Exceptions;
using Application.Payloads;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Auth.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Auth.Services
{
    internal class AuthService(ILogger<AuthService> logger,
        IBaseRepository<User> userRepository,
        IJwtTokenManager jwtTokenManager) : IAuthService

    {
        public async Task<string> SignUp(SignUpPayload payload)
        {
            var validation = AuthValidator.ValidateSignUp(payload);
            if (validation.Type is not ErrorType.Ok)
                throw new AppException(validation);

            var user = new User
            {
                Name = payload.Name,
                Email = payload.Email,
                Password = payload.Password,
                Role = "User"
            };

            if (user is null)
            {
                logger.LogWarning("SignUp: Bad Request");
                throw new AppException(AppError.Bad("Auth.SignUp", "Requested body payload seems invalid"));
            }

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();

            //if (registered is null)
            //    throw new Exception("This email already exists.");

            return user.Id;
        }

        public async Task<string> SignIn(SignInPayload payload)
        {
            if (payload is null)
            {
                logger.LogWarning("SignIn: Bad Request");
                throw new Exception("SignIn: Bad Request");
            }

            var validation = AuthValidator.ValidateSignIn(payload);
            if (validation.Type is not ErrorType.Ok)
                throw new AppException(validation);

            //if (validation.IsValid is false)
            //    return Error.Validation("AuthCommand.SignIn.InvalidForm",
            //        "SignIn form is invalid");

            var signInUser = (await userRepository.FindAsNoTrackAsync(
                x => x.Email == payload.Email 
                && x.Password == payload.Password))?.FirstOrDefault();
            if (signInUser is null)
                throw new AppException(AppError.Bad("SignIn.Credential.NotFound", "User credential didn't match"));

            var passHashWithSalt = jwtTokenManager.CreatePasswordHash(payload.Password, out byte[] passwordHash, out byte[] passwordSalt);
            signInUser.Password = passHashWithSalt;

            if (!jwtTokenManager.VerifyPasswordHash(signInUser.Password, passwordHash, passwordSalt))
                throw new AppException(AppError.Bad("SignIn.Credential.Wrong", "Wrong credential."));

            string token = jwtTokenManager.CreateNewToken(signInUser);
            return token;
        }

        public async Task<bool> SignOut(string token = "")
        {
            if (string.IsNullOrEmpty(token)) token = jwtTokenManager.GetHeaderToken();
            if (!jwtTokenManager.ValidateToken()) return true;
            var (principal, validatedToken) = jwtTokenManager.GetPrincipalFromToken(token);
            if (principal is null || validatedToken is null) return true;
            var userIdClaim = principal.Claims.FirstOrDefault(c =>
                c.Type == JwtRegisteredClaimNames.Sub ||
                c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null ||
                string.IsNullOrEmpty(userIdClaim.Value)) return true;
            var user = await userRepository.GetByIdAsync(userIdClaim.Value);
            if (user is null) return true;
            await userRepository.SaveChangesAsync();
            return true;
        }
    }
}
