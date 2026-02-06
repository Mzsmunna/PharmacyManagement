using Application.Abstractions;
using Application.Dtos;
using Application.Exceptions;
using Application.Extensions;
using Application.Payloads;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineDetailsController(IAppDBContext dBContext, IBaseRepository<DetailOverview> repository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repository.GetAllAsNoTrackAsync();
            //var data = result.ToModelList<DetailOverviewDto, DetailOverview>();      
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            //DetailOverviewDto? data = null;
            var result = await repository.GetByIdAsNoTrackAsync(id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(DetailOverview).Name + ".NotFound", $"No {typeof(DetailOverview).Name} available for id: " + id));
            //data = result.ToModel<DetailOverviewDto, DetailOverview>();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] FilterPayload payload)
        {
            //DetailOverviewDto? data = null;
            var result = await repository.FindAsNoTrackAsync(x => x.Title.Contains(payload.Name) || x.Id == payload.Id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(DetailOverview).Name} available for filter: " + payload.ToQueryString()));
            //data = result.ToModel<DetailOverviewDto, DetailOverview>();
            return Ok(result);
        }

        [HttpPost("{medicineId}")]
        public async Task<IActionResult> Add(string medicineId, [FromBody] DetailOverview payload)
        {
            DetailOverview? details = null;
            if (payload == null) throw new AppException(AppError.Bad(typeof(DetailOverview).Name + ".BadRequest"));
            if (string.IsNullOrEmpty(medicineId)) throw new AppException(AppError.Missing(typeof(DetailOverview).Name + ".Missing", "MedicineId is missing"));
            if (string.IsNullOrEmpty(payload.Title)) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Title is required"));
            if (string.IsNullOrEmpty(payload.Details)) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Details is required"));
            if (string.IsNullOrEmpty(payload.Id)) payload.Id = Guid.CreateVersion7().ToString();
            else details = await repository.GetByIdAsync(payload.Id);

            var medicine = await dBContext.Set<Medicine>()
                .Include(p => p.Details)
                .FirstAsync(p => p.Id == medicineId);
            
            var result = await repository.AddAsync(payload);
            var status = await repository.SaveChangesAsync();
            return Ok(payload.Id);
        }

        //[HttpPut("{medicineId}/{detailsId}")]
        //public async Task<IActionResult> Update(string medicineId, string detailsId, [FromBody] DetailOverview payload)
        [HttpPut("{medicineId}")]
        public async Task<IActionResult> Update(string medicineId, [FromBody] DetailOverview payload)
        {
            if (payload == null) throw new AppException(AppError.Bad(typeof(DetailOverview).Name + ".BadRequest"));
            if (string.IsNullOrEmpty(payload.Id)) throw new AppException(AppError.Missing(typeof(DetailOverview).Name + ".Missing", "Id is missing"));
            var result = await repository.GetByIdAsync(payload.Id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(DetailOverview).Name + ".NotFound", $"No {typeof(DetailOverview).Name} available for id: " + payload.Id));
            if (string.IsNullOrEmpty(medicineId)) throw new AppException(AppError.Missing(typeof(DetailOverview).Name + ".Missing", "MedicineId is missing"));
            if (string.IsNullOrEmpty(payload.Title)) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Title is required"));
            if (string.IsNullOrEmpty(payload.Details)) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Validation", "Details is required"));
            result.Title = payload.Title;
            result.Details = payload.Details;
            result.Type = payload.Type;
            result.Icon = payload.Icon;
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
