using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public required string InvoiceNumber { get; set; } = DomainHelper.GenerateInvoiceNumber();
        public required string CustomerId { get; set; }
        public int ItemsCount { get; set; } = 0;
        public int Total { get; set; } = 0;
        public int Discount { get; set; } = 0; // %
        public string Currency { get; set; } = string.Empty;

        // relations
        public User? User { get; set; }
        public ICollection<InvoiceItem> Items { get; set; } = [];
    }
}
