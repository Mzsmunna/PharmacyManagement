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
        string BatchId = "",
        string DetailId = "",
        string InvoiceId = "",
        string InvoiceNo = "",
        string Name = "",
        //string Role = "",
        string Email = "",
        string Phone = "",
        string Type = "",
        int PageNo = 1,
        int PageSize = 20
    );
}
