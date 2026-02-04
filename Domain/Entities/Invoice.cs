using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public required int Items { get; set; } = 0;
        public required decimal Total { get; set; } = 0;
        public string InvoiceNo { get; set; } = DomainHelper.GenerateInvoiceNumber();
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int Discount { get; set; } = 0; // %
        public string Currency { get; set; } = string.Empty;

        // relations
        public User? User { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = [];
    }
}
