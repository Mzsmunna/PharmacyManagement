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
    public class InvoicesController(IAppDBContext dBContext, IBaseRepository<Invoice> repository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repository.GetAllAsNoTrackAsync();
            //var data = result.ToModelList<InvoiceDto, Invoice>();      
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            //InvoiceDto? data = null;
            var result = await repository.GetByIdAsNoTrackAsync(id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Invoice).Name + ".NotFound", $"No {typeof(Invoice).Name} available for id: " + id));
            //data = result.ToModel<InvoiceDto, Invoice>();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] FilterPayload payload)
        {
            //InvoiceDto? data = null;
            var result = await repository.FindAsNoTrackAsync(x => x.InvoiceNo.Contains(payload.InvoiceNo) || x.CustomerId == payload.CustomerId || x.Id == payload.Id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Medicine).Name + ".NotFound", $"No {typeof(Invoice).Name} available for filter: " + payload.ToQueryString()));
            //data = result.ToModel<InvoiceDto, Invoice>();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(InvoicePayload payload)
        {
            if (payload == null || 
                payload.user == null || 
                payload.Items == null || 
                payload.Items.Count <= 0)
                throw new AppException(AppError.Bad(typeof(Invoice).Name + ".BadRequest"));

            var invoice = new Invoice { 
                Items = payload.Items.Count, 
                Total = 0,
                Currency = payload.Currency,
                CustomerId = payload.user.Id,
                CustomerName = payload.user.Name,
                CustomerPhone = payload.user.Phone,
            };

            foreach (var item in payload.Items)
            {
                if (item == null) continue;
                var medicineBatch = await dBContext.Set<MedicineBatch>()
                    //.Include(p => p.Medicine)
                    .FirstAsync(p => p.MedicineId == item.MedicineId && 
                                p.No == item.BatchNo);
                if (medicineBatch == null) throw new AppException(AppError.Missing(typeof(Medicine).Name + ".Missing", $"Medicine is missing for BatchNo: {item.BatchNo} & MedicineId: {item.MedicineId}"));
                if (medicineBatch.Quantity < item.Quantity) throw new AppException(AppError.Validation(typeof(MedicineBatch).Name + ".Quantity.Validation", "Medicine Quantity is insufficient"));
                
                InvoiceItem invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    InvoiceNo = invoice.InvoiceNo,
                    //Currency = invoice.Currency,
                    MedicineBatchId = medicineBatch.Id,
                    MedicineBatchNo = medicineBatch.No,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                };

                invoiceItem.Total = (item.Discount > 0) ? 
                    (medicineBatch.UnitPrice * (item.Discount / 100m)) * medicineBatch.Quantity : 
                    medicineBatch.UnitPrice * medicineBatch.Quantity;
                invoice.Total += invoiceItem.Total;
                medicineBatch.Quantity -= item.Quantity;
            }

            var status = await repository.SaveChangesAsync();
            return Ok(status);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var items = await dBContext.Set<InvoiceItem>()
                .Where(p => p.InvoiceId == id)
                //.Include(p => p.Invoice)
                .ToListAsync();
            foreach (var item in items)
            {
                if (item == null) continue;
                var medicineBatch = await dBContext.Set<MedicineBatch>()
                    //.Include(p => p.Medicine)
                    .FirstAsync(p => p.Id == item.MedicineBatchId && 
                                p.No == item.MedicineBatchNo);
                medicineBatch.Quantity += item.Quantity;
                item.IsDeleted = true;
                item.DeletedAt = DateTime.UtcNow;
            }

            var result = await repository.GetByIdAsync(id);
            if (result == null) throw new AppException(AppError.NotFound(typeof(Invoice).Name + ".NotFound", $"No {typeof(Invoice).Name} available for id: " + id));
            result.IsDeleted = true;
            result.DeletedAt = DateTime.UtcNow;
            var status = await repository.SaveChangesAsync();
            return Ok(status);
        }

    }
}
