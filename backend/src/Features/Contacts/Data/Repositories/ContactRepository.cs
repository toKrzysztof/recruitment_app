using RecruitmentApp.Features.Contacts.Api;
using RecruitmentApp.Features.Contacts.Data.Contracts;
using RecruitmentApp.Features.Contacts.Domain;
using RecruitmentApp.Shared.Data;
using RecruitmentApp.Shared.Data.Extensions;

namespace RecruitmentApp.Features.Contacts.Data.Repositories;

public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ContactRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<PagedList<Contact>> GetAllAsync(GetAllContactsQueryParams queryParams)
    {
      var query = DbSet.AsQueryable();

      var contacts = query
        .ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize);

      return await contacts;
    }
}
