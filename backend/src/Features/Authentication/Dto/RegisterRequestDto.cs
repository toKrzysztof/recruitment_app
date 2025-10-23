using System.ComponentModel.DataAnnotations;

namespace RecruitmentApp.Features.Authentication.Dto;

public class RegisterRequestDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
