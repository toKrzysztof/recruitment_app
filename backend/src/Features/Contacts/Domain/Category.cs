using RecruitmentApp.Shared.Domain.Contracts;

namespace RecruitmentApp.Features.Contacts.Domain;

public class Category : IEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
