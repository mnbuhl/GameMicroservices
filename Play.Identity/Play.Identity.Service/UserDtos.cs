using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Service
{
    public record UserDto(
        Guid Id,
        string Username,
        string Email,
        decimal Balance,
        DateTime CreatedDate
    );

    public record UpdateUserDto(
        [Required, EmailAddress] string Email,
        [Range(0, 1_000_000)] decimal Balance
    );
}