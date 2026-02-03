using Application.Payloads;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions
{
    public interface IAuthService
    {
        Task<string> SignUp(SignUpPayload payload);
        Task<string> SignIn(SignInPayload payload);
        Task<bool> SignOut(string token = "");
    }
}
