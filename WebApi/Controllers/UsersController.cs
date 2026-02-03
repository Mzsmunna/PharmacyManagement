using Application.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //[Authorize(Roles = AppRole.User)]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IBaseRepository<User> userRepository) : ControllerBase
    {
        //[HttpGet, Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await userRepository.GetAllAsync();
            return Ok(result);
        }
    }
}
