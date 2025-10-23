using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Shared.Domain.Contracts;

namespace RecruitmentApp.Features.Contacts.Domain;

[Index(nameof(Name), IsUnique = true)]
public class Subcategory : IEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Category Category { get; set; }
}
