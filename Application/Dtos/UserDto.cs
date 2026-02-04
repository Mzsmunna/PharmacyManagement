using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos
{
    public record UserDto(
        [Required] string Id,
        [Required] string Name,
        //string FirstName,
        //string LastName,
        string Gender,
        DateTime? DOB,
        //string Role,
        //string Username,
        [Required] string Email,
        string Phone,
        string Address,
        string? Img
    );
}
