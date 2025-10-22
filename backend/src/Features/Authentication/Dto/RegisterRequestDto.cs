using System.ComponentModel.DataAnnotations;

namespace RecruitmentApp.Features.Authentication.Dto;

public class RegisterRequestDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}
