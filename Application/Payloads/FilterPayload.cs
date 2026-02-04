using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Payloads
{
    public record FilterPayload(
        string Id = "",
        string UserId = "",
        string CustomerId = "",
        string MedicineId = "",
        string InvoiceId = "",
        string InvoiceNo = "",
        string Name = "",
        //string Role = "",
        string Email = "",
        string Phone = "",
        string Type = "",
        string SKU = "",
        int PageNo = 1,
        int PageSize = 20
    );
}
