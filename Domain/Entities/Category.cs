using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string Title { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}
