using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class DetailOverview : BaseEntity
    {
        public required string Title { get; set; }
        public string Details { get; set; } = string.Empty;
        public required string MedId { get; set; }
        public required string CatId { get; set; }
        public string? Type { get; set; } = string.Empty; // e.g., "paragraph", "list", etc.
        public string? Icon { get; set; } = string.Empty;
    }
}
