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
        public int BatchNo { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public int UnitPrice { get; set; } = 0;
        public int OrderNo { get; set; } = 0;
    }
}
