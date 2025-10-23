using RecruitmentApp.Features.Contacts.Domain;

namespace RecruitmentApp.Features.Contacts.Dto;

public class ContactDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required CategoryDto Category { get; set; }
    public SubcategoryDto? Subcategory { get; set; }
}
