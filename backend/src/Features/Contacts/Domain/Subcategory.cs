using RecruitmentApp.Shared.Domain.Contracts;

namespace RecruitmentApp.Features.Contacts.Domain;

public class Subcategory : IEntity
{
    public int Id { get; set; }
    public required Category Category { get; set; }
}
