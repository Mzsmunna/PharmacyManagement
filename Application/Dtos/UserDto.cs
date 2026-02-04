using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos
{
    public record UserDto(
        [Required] string Id,
        [Required] string Name,
        [Required] string Email,
        string Phone,
        string Address,
        string? Img
    );
}
