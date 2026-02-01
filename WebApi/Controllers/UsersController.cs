using Application.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //[Authorize(Roles = AppRole.User)]
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IBaseRepository<User> userSqlRepo) : ControllerBase
    {
        //[HttpGet, Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await userSqlRepo.GetAllAsync();
            return Ok(result);
        }
    }
}
