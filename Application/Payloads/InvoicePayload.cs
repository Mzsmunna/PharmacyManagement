using Application.Dtos;
using Domain.Entities;
using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Payloads
{
    public record InvoicePayload(
        List<ItemPayload> Items,
        //UserDto user,
        int Discount = 0, // %
        string Currency = "",
        string CustomerName = "",
        string CustomerPhone = ""
    );
}
