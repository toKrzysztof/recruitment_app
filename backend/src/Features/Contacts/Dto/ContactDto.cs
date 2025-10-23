using RecruitmentApp.Features.Contacts.Domain;

namespace RecruitmentApp.Features.Contacts.Dto;

public class ContactDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required Category Category { get; set; }
    public Subcategory? Subcategory { get; set; }
}
