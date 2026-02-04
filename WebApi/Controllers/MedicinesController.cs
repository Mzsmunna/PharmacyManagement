using Application.Abstractions;
using Application.Dtos;
using Application.Exceptions;
using Application.Extensions;
using Application.Payloads;
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
        public async Task<IActionResult> GetById(string id)
        {
            //MedicineDto? data = null;
            var result = await repository.GetByIdAsNoTrackAsync(id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Medicine).Name} available for id: " + id));
            //data = result.ToModel<MedicineDto, Medicine>();
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
            if (payload == null) throw new AppException(AppError.Bad(typeof(MedicineBatch).Name + ".BadRequest"));
            if (string.IsNullOrEmpty(payload.Name)) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Name is required"));
            if (string.IsNullOrEmpty(payload.Type)) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Type is required"));
            payload.Id = Guid.CreateVersion7().ToString();
            var result = await repository.AddAsync(payload);
            var status = await repository.SaveChangesAsync();
            return Ok(status);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Medicine payload)
        {
            if (payload == null) throw new AppException(AppError.Bad(typeof(MedicineBatch).Name + ".BadRequest"));
            if (string.IsNullOrEmpty(payload.Id)) throw new AppException(AppError.Missing(typeof(MedicineBatch).Name + ".Missing", "Need Id to update"));
            if (string.IsNullOrEmpty(payload.Name)) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Name is required"));
            if (string.IsNullOrEmpty(payload.Type)) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Type is required"));
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
