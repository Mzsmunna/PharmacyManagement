using Application.Abstractions;
using Application.Dtos;
using Application.Exceptions;
using Application.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController(IBaseRepository<Medicine> repository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repository.GetAllAsNoTrackAsync();
            //var data = result.ToModelList<MedicineDto, Medicine>();      
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            //MedicineDto? data = null;
            var result = await repository.GetByIdAsNoTrackAsync(id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Medicine).Name} available for id: " + id));
            //data = result.ToModel<MedicineDto, Medicine>();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Add(Medicine payload)
        {
            payload.Id = Guid.CreateVersion7().ToString();
            var result = await repository.AddAsync(payload);
            var status = await repository.SaveChangesAsync();
            return Ok(status);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Medicine payload)
        {
            var result = await repository.GetByIdAsync(payload.Id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Medicine).Name} available for id: " + payload.Id));
            result.Name = payload.Name;
            result.Description = payload.Description;
            result.Type = payload.Type;
            result.Image = payload.Image;
            result.SKU = payload.SKU;
            await repository.SaveChangesAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await repository.DeleteByIdAsync(id);
            return Ok(result);
        }
    }
}
