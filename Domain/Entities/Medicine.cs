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
        public string Image { get; set; } = string.Empty;
        //public string SKU { get; set; } = string.Empty;
        //public string Barcode { get; set; } = string.Empty;
        //public string BrandName { get; set; } = string.Empty;
        //public string Manufacturer { get; set; } = string.Empty;

        // relations
        public ICollection<DetailOverview> Details { get; set; } = [];
        public ICollection<MedicineBatch> Batches { get; set; } = [];
    }
}
