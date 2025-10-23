using RecruitmentApp.Features.Contacts.Api;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Shared.Data;
using RecruitmentApp.Shared.Data.Contracts;

namespace RecruitmentApp.Features.Contacts.Data.Contracts;

public interface IContactRepository : IRepositoryBase<Contact>
{
    public Task<PagedList<Contact>> GetAllAsync(GetAllContactsQueryParams queryParams);
    public Task<Contact?> GetByEmailAsync(string email);
}
