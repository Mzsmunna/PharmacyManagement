using Application.Abstractions;
using Application.Payloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{  
    [AllowAnonymous]
    [Route("api/[controller]/[action]")] //[Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> Register(SignUpPayload payload)
        {
            var result = await authService.SignUp(payload);
            return string.IsNullOrEmpty(result) ?
                    BadRequest() :
                    Ok(result);       
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> Login(SignInPayload payload)
        {
            var result = await authService.SignIn(payload);
            return string.IsNullOrEmpty(result) ?
                    Unauthorized() :
                    Ok(result);
        }

        [HttpPost]
        [ActionName("Logout")]
        public async Task<IActionResult> Logout()
        {
            var isSuccess = await authService.SignOut();
            return Ok(isSuccess);
        }
    }
}
