using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Shared.Domain.Contracts;

namespace RecruitmentApp.Features.Contacts.Domain;

[Index(nameof(Email), IsUnique = true)]
public class Contact : IEntity
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required Category Category { get; set; }
    public Subcategory? Subcategory { get; set; }
}
