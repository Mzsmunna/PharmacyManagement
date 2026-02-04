using Application.Abstractions;
using Application.Dtos;
using Application.Exceptions;
using Application.Extensions;
using Application.Payloads;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    //[Authorize(Roles = AppRole.User)]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IBaseRepository<User> userRepository) : ControllerBase
    {
        //[HttpGet, Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await userRepository.GetAllAsNoTrackAsync();
            var data = result.ToModelList<UserDto, User>();      
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            UserDto? data = null;
            var result = await userRepository.GetByIdAsNoTrackAsync(id);
            if (result == null) throw new AppException(AppError.NotFound("User.NotFound", "No user available for id: " + id));
            data = result.ToModel<UserDto, User>();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDto user)
        {
            var result = await userRepository.GetByIdAsync(user.Id);
            if (result == null) throw new AppException(AppError.NotFound("User.NotFound", "No user available for id: " + user.Id));
            result.Name = user.Name;
            await userRepository.SaveChangesAsync();

            //var result = await userRepository.GetByIdAsNoTrackAsync(user.Id);
            //result = user.ToEntity<User, UserDto>();
            //await userRepository.UpdateAsync(result);
            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await userRepository.GetByIdAsync(id);
            if (result == null) throw new AppException(AppError.Missing("User.Missing", "Missing user id: " + id));
            result.IsDeleted = true;
            await userRepository.UpdateAsync(result);
            return Ok(result.IsDeleted);
        }
    }
}
