using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Shared.Data.Contracts;

namespace RecruitmentApp.Features.Contacts.Data.Contracts;

public interface ISubcategoryRepository : IRepositoryBase<Subcategory>
{
    public Task<Subcategory?> GetByName(string name);
}
