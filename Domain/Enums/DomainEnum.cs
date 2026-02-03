using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum MedicineType
    {
        Tablet,
        Capsule,
        Syrup,
        Injection,
        Cream,
        Ointment,
        Drops,
        Inhaler,
        Suppository,
    }

    public enum ErrorType
    {
        Ok,
        Null,
        Bad,
        Unauthorized,
        Forbidden,
        NotFound,
        Missing,
        Conflict,
        Validation,
        RateLimit,
        Server,
        Network
    }
}
