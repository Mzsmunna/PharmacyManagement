using Application.Abstractions;
using Application.Dtos;
using Application.Exceptions;
using Application.Extensions;
using Application.Payloads;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController(IAppDBContext dBContext, IBaseRepository<Medicine> repository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repository.GetAllAsNoTrackAsync();
            //var data = result.ToModelList<MedicineDto, Medicine>();      
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            //MedicineDto? data = null;
            var result = await repository.GetByIdAsNoTrackAsync(id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Medicine).Name} available for id: " + id));
            //data = result.ToModel<MedicineDto, Medicine>();
            return Ok(result);
        }

        [HttpGet("Batches/{id}")]
        public async Task<IActionResult> GetWithJoins(string id)
        {
            var result = await dBContext.Set<Medicine>().Where(x => x.Id == id).Include(y => y.Batches).ToListAsync();
            if (result == null || result.Count <= 0) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Medicine).Name} available for id: " + id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] FilterPayload payload)
        {
            //MedicineDto? data = null;
            var result = await repository.FindAsNoTrackAsync(x => x.Id == payload.Id || x.Name.Contains(payload.Name));
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Medicine).Name} available for filter: " + payload.ToQueryString()));
            //data = result.ToModel<MedicineDto, Medicine>();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Medicine payload)
        {
            if (payload == null) throw new AppException(AppError.Bad(typeof(Medicine).Name + ".BadRequest"));
            if (string.IsNullOrEmpty(payload.Name)) throw new AppException(AppError.Validation(typeof(Medicine).Name + ".Validation", "Name is required"));
            if (string.IsNullOrEmpty(payload.Type)) throw new AppException(AppError.Validation(typeof(Medicine).Name + ".Validation", "Type is required"));
            var existing = (await repository.FindAsNoTrackAsync(x => x.Name.Equals(payload.Name) && x.Type.Equals(payload.Type)))?.FirstOrDefault();
            if (existing != null) throw new AppException(AppError.Conflict(typeof(Medicine).Name + ".Conflict", "Medicine already exist"));
            payload.Id = Guid.CreateVersion7().ToString();
            var result = await repository.AddAsync(payload);
            var status = await repository.SaveChangesAsync();
            return Ok(payload.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Medicine payload)
        {
            if (payload == null) throw new AppException(AppError.Bad(typeof(Medicine).Name + ".BadRequest"));
            if (string.IsNullOrEmpty(payload.Id)) throw new AppException(AppError.Missing(typeof(Medicine).Name + ".Missing", "Need Id to update"));
            if (string.IsNullOrEmpty(payload.Name)) throw new AppException(AppError.Validation(typeof(Medicine).Name + ".Validation", "Name is required"));
            if (string.IsNullOrEmpty(payload.Type)) throw new AppException(AppError.Validation(typeof(Medicine).Name + ".Validation", "Type is required"));
            var result = await repository.GetByIdAsync(payload.Id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Medicine).Name} available for id: " + payload.Id));
            result.Name = payload.Name;
            result.Description = payload.Description;
            result.Type = payload.Type;
            result.Image = payload.Image;
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
