using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Payloads
{
    public record SignUpPayload(
        [Required] string Name,
        //string FirstName,
        //string LastName,
        string Gender,
        DateTime? DOB,
        //string Role,
        //string Username,
        [Required] string Email,
        [Required] string Password,
        [Required] string ConfirmPassword,
        string Phone,
        string Address,
        string? Img
    );
}
