using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class MedicineSale : BaseEntity
    {
        public string InvoiceNumber { get; set; } = string.Empty; //DomainHelper.GenerateInvoiceNumber();
        public required string CustomerId { get; set; }
        public required string MedicineId { get; set; }
    }
}
