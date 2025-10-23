namespace RecruitmentApp.Features.Contacts.Dto;

public class ContactDetailsDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required CategoryDto CategoryDto { get; set; }
    public SubcategoryDto? SubcategoryDto { get; set; }
}
