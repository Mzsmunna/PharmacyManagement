using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class MedicineBatch : BaseEntity
    {
        public required string BatchNo { get; set; }
        public required string MedicineId { get; set; }
        public int Quantity { get; set; } = 0;
        public int UnitPrice { get; set; } = 0;
        public int Discount { get; set; } = 0; // %
        public string Currency { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;

        // relations
        public Medicine? Medicine { get; set; }
    }
}
