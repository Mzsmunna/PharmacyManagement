using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Helpers
{
    public static class DomainHelper
    {
        public static string GenerateInvoiceNumber()
        {
            return $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
