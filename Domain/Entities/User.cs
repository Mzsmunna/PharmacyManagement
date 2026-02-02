using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public required string Password { get; set; }
        public required string Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
