using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Payloads
{
    public record ItemPayload(
        string MedicineId = "",
        string BatchNo = "",
        int Quantity = 0, // %
        int UnitPrice = 0, // %
        int Discount = 0 // %
    );
}
