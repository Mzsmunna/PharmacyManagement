using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class InvoiceItem : BaseEntity
    {
        //public required string MedicineId { get; set; }
        public required string InvoiceId { get; set; }
        public required string InvoiceNo { get; set; }
        
        public required string MedicineBatchId { get; set; }
        public required string MedicineBatchNo { get; set; }
        public required int Quantity { get; set; } = 0;
        public required int UnitPrice { get; set; } = 0;
        public int Discount { get; set; } = 0; // %
        public decimal Total { get; set; } = 0;
        //public string Currency { get; set; } = string.Empty;

        // relations
        //public Medicine? Medicine { get; set; }
        public Invoice? Invoice { get; set; }
        public MedicineBatch? Batch { get; set; }
    }
}
