using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Medicine : BaseEntity
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string BatchNo { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public int UnitPrice { get; set; } = 0;
        public int Discount { get; set; } = 0; // %
        public string Currency { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;
    }
}
