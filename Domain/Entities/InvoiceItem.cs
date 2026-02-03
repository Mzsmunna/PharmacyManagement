using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class InvoiceItem : BaseEntity
    {
        public required string InvoiceId { get; set; }
        public required string ItemId { get; set; }
        public int Quantity { get; set; } = 0;
        public int UnitPrice { get; set; } = 0;
        public int Discount { get; set; } = 0; // %
        public string Currency { get; set; } = string.Empty;

        // relations
        public Medicine? Medicine { get; set; }
        public Invoice? Invoice { get; set; }
    }
}
