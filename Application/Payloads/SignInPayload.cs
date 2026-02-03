using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Payloads
{
    public record SignInPayload(
        [Required, EmailAddress] string Email,
        [Required, MinLength(3)] string Password
    );
}
