using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Shared.Data.Contracts;

namespace RecruitmentApp.Features.Contacts.Data.Contracts;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    public Task<Category?> GetByNameAsync(string name);
}
