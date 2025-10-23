using RecruitmentApp.Features.Contacts.Data.Contracts;

namespace RecruitmentApp.Shared.Data.Contracts;

public interface IUnitOfWork
{
    IContactRepository Contacts { get; }
    Task<bool> SaveChangesAsync();
}
