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
    public class MedicineBatchesController(IBaseRepository<MedicineBatch> repository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repository.GetAllAsNoTrackAsync();
            //var data = result.ToModelList<MedicineBatchDto, MedicineBatch>();      
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            //MedicineDto? data = null;
            var result = await repository.GetByIdAsNoTrackAsync(id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(MedicineBatch).Name + ".NotFound", $"No {typeof(MedicineBatch).Name} available for id: " + id));
            //data = result.ToModel<MedicineBatchDto, MedicineBatch>();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] FilterPayload payload)
        {
            //MedicineDto? data = null;
            var result = await repository.FindAsNoTrackAsync(x => x.MedicineId == payload.MedicineId || x.Id == payload.Id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(MedicineBatch).Name} available for filter: " + payload.ToQueryString()));
            //data = result.ToModel<MedicineBatchDto, MedicineBatch>();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MedicineBatch payload)
        {
            if (payload == null) throw new AppException(AppError.Bad(typeof(MedicineBatch).Name + ".BadRequest"));
            if (string.IsNullOrEmpty(payload.MedicineId)) throw new AppException(AppError.Missing(typeof(MedicineBatch).Name + ".Missing", "MedicineId is missing"));
            if (payload.Quantity <= 0) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Quantity can't be 0"));
            if (payload.UnitPrice <= 0) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "UnitPrice can't be 0"));
            
            payload.Id = Guid.CreateVersion7().ToString();
            var result = await repository.AddAsync(payload);
            var status = await repository.SaveChangesAsync();
            return Ok(payload.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MedicineBatch payload)
        {
            if (payload == null) throw new AppException(AppError.Bad(typeof(MedicineBatch).Name + ".BadRequest"));
            if (string.IsNullOrEmpty(payload.Id)) throw new AppException(AppError.Missing(typeof(MedicineBatch).Name + ".Missing", "Need Id to update"));
            var result = await repository.GetByIdAsync(payload.Id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(MedicineBatch).Name + ".NotFound", $"No {typeof(MedicineBatch).Name} available for id: " + payload.Id));
            if (string.IsNullOrEmpty(payload.MedicineId)) throw new AppException(AppError.Missing(typeof(MedicineBatch).Name + ".Missing", "MedicineId is seems missing"));
            if (payload.Quantity <= 0) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Quantity can't be 0"));
            if (payload.UnitPrice <= 0) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "UnitPrice can't be 0"));
            
            result.No = payload.No;
            result.MedicineId = payload.MedicineId;
            result.Quantity = payload.Quantity;
            result.UnitPrice = payload.UnitPrice;
            result.Discount = payload.Discount;
            result.Currency = payload.Currency;
            result.ExpiryDate = payload.ExpiryDate.ToUniversalTime();
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
