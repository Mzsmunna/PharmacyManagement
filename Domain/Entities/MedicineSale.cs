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
        public int Quantity { get; set; } = 0;
        public int UnitPrice { get; set; } = 0;
        public int Discount { get; set; } = 0; // %
        public string Currency { get; set; } = string.Empty;
        // related tables
        public Medicine? Medicine { get; set; }
        public User? User { get; set; }
    }
}
