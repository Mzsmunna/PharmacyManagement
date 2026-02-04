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
    public class InvoiceItemsController(IBaseRepository<InvoiceItem> repository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repository.GetAllAsNoTrackAsync();
            //var data = result.ToModelList<InvoiceItemDto, InvoiceItem>();      
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            //InvoiceItemDto? data = null;
            var result = await repository.GetByIdAsNoTrackAsync(id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(InvoiceItem).Name + ".NotFound", $"No {typeof(InvoiceItem).Name} available for id: " + id));
            //data = result.ToModel<InvoiceItemDto, InvoiceItem>();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] FilterPayload payload)
        {
            //InvoiceItemDto? data = null;
            var result = await repository.FindAsNoTrackAsync(x => x.InvoiceNo.Contains(payload.InvoiceNo) || x.InvoiceId == payload.InvoiceId || x.Id == payload.Id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Invoice).Name} available for filter: " + payload.ToQueryString()));
            //data = result.ToModel<InvoiceItemDto, InvoiceItem>();
            return Ok(result);
        }
    }
}
