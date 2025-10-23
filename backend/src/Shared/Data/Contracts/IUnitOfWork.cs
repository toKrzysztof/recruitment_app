using RecruitmentApp.Features.Contacts.Data.Contracts;

namespace RecruitmentApp.Shared.Data.Contracts;

public interface IUnitOfWork
{
    IContactRepository Contacts { get; }
    ICategoryRepository Categories { get; }
    ISubcategoryRepository Subcategories { get; }
    Task<bool> SaveChangesAsync();
}
